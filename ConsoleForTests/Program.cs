using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ConsoleForTests
{
    public class DomclickApi : IDisposable
    {
        private readonly List<DomclickCatalogueFetchResult> _results = [];
        private RestClient _client = null!;
        private const string Sault = "ad65f331b02b90d868cbdd660d82aba0";
        System.Net.CookieCollection _cookies = new System.Net.CookieCollection();
        private string researchApiToken = string.Empty;
        const string phonePattern = @"""phone""\s*:\s*""(?<phone>[^""]+)""";
        const string tokenPattern = @"""result""\s*:\s*""(?<result>[^""]+)""";
        private static readonly Regex phoneRegex = new Regex(phonePattern, RegexOptions.Compiled);
        private static readonly Regex tokenRegex = new Regex(tokenPattern, RegexOptions.Compiled);

        public DomclickApi()
        {
            ServicePointManager.ServerCertificateValidationCallback += (
                sender,
                certificate,
                chain,
                sslPolicyErrors
            ) =>
            {
                return true;
            };
            UpdateRestClient();
            // GetAsync("https://api.domclick.ru/core/no-auth-zone/api/v1/ensure_session").Wait();
            // GetAsync("https://ipoteka.domclick.ru/mobile/v1/feature_toggles").Wait();
        }

        private void UpdateRestClient()
        {
            _client = new RestClient();
            _client.AddDefaultHeader("X-Service", "true");
            _client.AddDefaultHeader("Connection", "Keep-Alive");
            _client.AddDefaultHeader(
                "User-Agent",
                "Android; 12; Google; google_pixel_5; 8.72.0; 8720006; ; NONAUTH"
            );
        }

        public async Task EnsureSessionAsync() =>
            await GetAsync("https://api.domclick.ru/core/no-auth-zone/api/v1/ensure_session");

        public async Task EnsureNsSessionAsync() =>
            GetAsync("https://ipoteka.domclick.ru/mobile/v1/feature_toggles");

        private async Task<string> GetAsync(string url)
        {
            UpdateHeaders(url);
            var request = new RestRequest(url, Method.Get);
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(string.Join("\n", _client.DefaultParameters));
            if (response.Cookies != null)
            {
                foreach (System.Net.Cookie cookie in response.Cookies)
                {
                    _cookies.Add(cookie);
                }
                var d = response.Cookies.First();
            }
            else
            {
                Console.WriteLine("Cookies are null");
            }
            return response.Content;
        }

        public async Task InitializeResults()
        {
            foreach (var result in _results)
            {
                await GetResearchApiToken(result);
                await GetPhoneNumber(result);
                Thread.Sleep(5000);
                Console.WriteLine("Advertisement completed");
            }
        }

        public async Task GetItemsFromCatalogue()
        {
            DomclickCatalogueFetchResultFactory factory = new();
            int limit = 0;
            int offset = 0;
            bool isNotFirstFetch = true;
            while (true)
            {
                if (offset + 20 < limit || isNotFirstFetch)
                {
                    string url = CreateCatalogueItemsRequestUrl(offset);
                    UpdateHeaders(url);
                    var request = new RestRequest(url, Method.Get);
                    foreach (System.Net.Cookie cookie in _cookies)
                    {
                        request = request.AddCookie(
                            cookie.Name,
                            cookie.Value,
                            cookie.Path,
                            cookie.Domain
                        );
                    }
                    var response = await _client.ExecuteAsync(request);
                    string content = response.Content;
                    if (!string.IsNullOrEmpty(content))
                    {
                        offset = UpdateOffset(offset);
                        if (!isNotFirstFetch)
                        {
                            limit = UpdateMaxCount(content, limit);
                        }
                        isNotFirstFetch = false;
                        _results.AddRange(factory.Create(content));
                        Console.WriteLine("Catalogue results added");
                    }
                    else
                    {
                        Console.WriteLine("Items from catalogue response is terminated");
                        break;
                    }
                    Thread.Sleep(5000);
                }
                else
                    break;
            }
        }

        private string CreateCatalogueItemsRequestUrl(int offset)
        {
            string url =
                $"https://bff-search-web.domclick.ru/api/offers/v1?address=6b2a4aad-bd39-4982-9ee0-2cc25449964b&offset={offset}&limit=20&sort=qi&sort_dir=desc&deal_type=sale&category=living&offer_type=flat&offer_type=layout&aids=650885&sort_by_tariff_date=1";
            return url;
        }

        public async Task GetResearchApiToken(DomclickCatalogueFetchResult result)
        {
            string url = $"https://bff-search-web.domclick.ru/api/phone/token/v1/{result.Id}";
            UpdateHeaders(url);
            RestRequest request = new RestRequest(url, Method.Get)
                .AddHeader("Accept", "application/json, text/plain, */*")
                .AddHeader("Referer", "https://krasnoyarsk.domclick.ru/");
            foreach (System.Net.Cookie cookie in _cookies)
            {
                request = request.AddCookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain);
            }
            var response = await _client.ExecuteAsync(request);
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                researchApiToken = ExtractToken(response.Content);
                Console.WriteLine(researchApiToken);
            }
            else
            {
                Console.WriteLine("No research API token found.");
            }
        }

        public async Task GetPhoneNumber(DomclickCatalogueFetchResult result)
        {
            string url = $"https://bff-search-web.domclick.ru/api/phone/get/v3/{result.Id}";
            UpdateHeaders(url);
            RestRequest request = new RestRequest(url, Method.Get)
                .AddHeader("referer", "https://krasnoyarsk.domclick.ru/")
                .AddHeader("research-api-token", researchApiToken);
            foreach (System.Net.Cookie cookie in _cookies)
            {
                request = request.AddCookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain);
            }
            var response = await _client.ExecuteAsync(request);
            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                string phone = ExtractPhone(response.Content);
                Console.WriteLine(phone);
                result.PhoneNumber = phone;
            }
            else
            {
                Console.WriteLine("No phone found.");
            }
        }

        private int UpdateOffset(int currentOffset)
        {
            return currentOffset += 20;
        }

        private int UpdateMaxCount(string? response, int currentLimit)
        {
            if (string.IsNullOrWhiteSpace(response))
                return currentLimit;
            if (currentLimit != 0)
                return currentLimit;
            JObject jsonObject = JObject.Parse(response);
            JToken? result = jsonObject["result"];
            if (result == null)
                return currentLimit;
            JToken? pagination = result["pagination"];
            if (pagination == null)
                return currentLimit;
            JToken? total = pagination["total"];
            if (total == null)
                return currentLimit;
            currentLimit = int.Parse(total.ToString());
            return currentLimit;
        }

        private void UpdateHeaders(string url)
        {
            string timestamp = ((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString();
            string hash = ComputeMD5Hash(Sault + url + timestamp);
            UpdateRestClient();
            _client.AddDefaultHeader("Timestamp", timestamp);
            _client.AddDefaultHeader("Hash", $"v1:{hash}");
        }

        private string ComputeMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private static string ExtractPhone(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return string.Empty;
            Match match = phoneRegex.Match(response);
            return match.Success ? match.Groups["phone"].Value : string.Empty;
        }

        private static string ExtractToken(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return string.Empty;
            Match match = tokenRegex.Match(response);
            return match.Success ? match.Groups["result"].Value : string.Empty;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    class Program
    {
        static void Main()
        {
            Test().Wait();
        }

        static async Task Test()
        {
            using (DomclickApi api = new DomclickApi())
            {
                // await api.GetResearchApiToken();
                // await api.GetPhoneNumber();
                await api.EnsureSessionAsync();
                await api.EnsureNsSessionAsync();
                await api.GetItemsFromCatalogue();
                await api.InitializeResults();
            }
        }
    }
}
