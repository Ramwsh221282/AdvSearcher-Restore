using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal sealed class DomclickPhoneInitializer : IDisposable
{
    private readonly DomclickFetchResult _result;
    private RestClient _client = new RestClient();
    private const string Sault = "ad65f331b02b90d868cbdd660d82aba0";
    System.Net.CookieCollection _cookies = new System.Net.CookieCollection();
    private string researchApiToken = string.Empty;
    const string phonePattern = @"""phone""\s*:\s*""(?<phone>[^""]+)""";
    const string tokenPattern = @"""result""\s*:\s*""(?<result>[^""]+)""";
    private static readonly Regex phoneRegex = new Regex(phonePattern, RegexOptions.Compiled);
    private static readonly Regex tokenRegex = new Regex(tokenPattern, RegexOptions.Compiled);

    public DomclickPhoneInitializer(DomclickFetchResult result)
    {
        _result = result;
        SetDefaultHeaders();
        GetAsync("https://api.domclick.ru/core/no-auth-zone/api/v1/ensure_session").Wait();
        GetAsync("https://ipoteka.domclick.ru/mobile/v1/feature_toggles").Wait();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private async Task<string> GetAsync(string url)
    {
        UpdateHeaders(url);
        var request = new RestRequest(url, Method.Get);
        var response = await _client.ExecuteAsync(request);

        foreach (System.Net.Cookie cookie in response.Cookies)
        {
            _cookies.Add(cookie);
        }
        return response.Content;
    }

    public async Task GetResearchApiToken()
    {
        string url = $"https://bff-search-web.domclick.ru/api/phone/token/v1/{_result.Id}";
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
            researchApiToken = ExtractToken(response.Content);
    }

    public async Task GetPhoneNumber()
    {
        string url = $"https://bff-search-web.domclick.ru/api/phone/get/v3/{_result.Id}";
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
            _result.PhoneNumber = phone;
        }
    }

    private void SetDefaultHeaders()
    {
        _client.AddDefaultHeader("X-Service", "true");
        _client.AddDefaultHeader("Connection", "Keep-Alive");
        _client.AddDefaultHeader(
            "User-Agent",
            "Android; 12; Google; google_pixel_5; 8.72.0; 8720006; ; NONAUTH"
        );
    }

    private void UpdateHeaders(string url)
    {
        string timestamp = ((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString();
        string hash = ComputeMD5Hash(Sault + url + timestamp);
        _client = new RestClient();
        SetDefaultHeaders();
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
}
