using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace ConsoleForTests
{
    public class WebDriverCookies
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public static string CreateCookieHeaderFromList(List<WebDriverCookies> cookies)
        {
            WebDriverCookies[] filtered = cookies
                .Where(c => c.Domain.Contains(".domclick.ru"))
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var cookie in filtered)
            {
                sb.Append(CreateCookieForm(cookie));
            }
            return sb.ToString().Trim();
        }

        private static string CreateCookieForm(WebDriverCookies cookie)
        {
            string form = $"{cookie.Name}={cookie.Value}; ";
            return form;
        }
    }

    class Program
    {
        static void Main()
        {
            string result = new DriverManager().SetUpDriver(
                new ChromeConfig(),
                VersionResolveStrategy.MatchingBrowser
            );

            CleanFromChromeProcesses();
            CleanFromChromeDriverProcesses();

            const string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            const string chromeDriverPath =
                @"C:\Program Files\Google\Chrome\Application\chromedriver.exe";

            const string argument = "--remote-debugging-port=8888 https://krasnoyarsk.domclick.ru";
            var process = Process.Start(chromePath, argument);

            while (true)
            {
                if (Process.GetProcessesByName("chrome").Length == 0)
                    continue;
                break;
            }

            var options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:8888";
            IWebDriver driver = new ChromeDriver(
                chromeDriverDirectory: chromeDriverPath,
                options: options
            );
            driver
                .Navigate()
                .GoToUrl("https://krasnoyarsk.domclick.ru/card/sale__flat__2062607424");

            List<WebDriverCookies> cookies = [];
            cookies = driver
                .Manage()
                .Cookies.AllCookies.Select(cookie => new WebDriverCookies()
                {
                    Name = cookie.Name,
                    Value = cookie.Value,
                    Domain = cookie.Domain,
                    Path = cookie.Path,
                })
                .ToList();

            string cookieHeaderValue = WebDriverCookies.CreateCookieHeaderFromList(cookies);

            const string UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";
            var requestOptions = new RestClientOptions() { UserAgent = UserAgent };
            var _instance = new RestClient(requestOptions);

            string requestUrl = "https://offer-card.domclick.ru/api/v3/public_request/2062607424";
            var tokenRequest = new RestRequest(requestUrl)
                .AddHeader("Accept", "application/json, text/plan, */*")
                .AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
                .AddHeader("Connection", "keep-alive")
                .AddHeader("Cookie", cookieHeaderValue)
                .AddHeader("Origin", "https://krasnoyarsk.domclick.ru")
                .AddHeader("Referer", "https://krasnoyarsk.domclick.ru/")
                .AddHeader("Sec-Fetch-Dest", "empty")
                .AddHeader("Sec-Fetch-Mode", "cors")
                .AddHeader("Sec-Fetch-Site", "same-site")
                .AddHeader(
                    "sec-ch-ua",
                    "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\""
                )
                .AddHeader("sec-ch-ua-mobile", "?0")
                .AddHeader("sec-ch-ua-platform", "\"Windows\"");

            var tokenResponse = _instance.ExecuteAsync(tokenRequest).Result;
            Thread.Sleep(5000);
            string? Content = tokenResponse.Content;
            string token = ExtractToken(Content);

            var phoneRequest = new RestRequest(
                "https://offer-card.domclick.ru/api/v3/offers/phone/2062607424"
            )
                .AddHeader("Accept", "application/json, text/plain, */*")
                .AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
                .AddHeader("Connection", "keep-alive")
                .AddHeader("Cookie", cookieHeaderValue)
                .AddHeader("Origin", "https://krasnoyarsk.domclick.ru")
                .AddHeader("Referer", "https://krasnoyarsk.domclick.ru/")
                .AddHeader("Sec-Fetch-Dest", "empty")
                .AddHeader("Sec-Fetch-Mode", "cors")
                .AddHeader("Sec-Fetch-Site", "same-site")
                .AddHeader("research-api-token", token)
                .AddHeader(
                    "sec-ch-ua",
                    "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\""
                )
                .AddHeader("sec-ch-ua-mobile", "?0")
                .AddHeader("sec-ch-ua-platform", "\"Windows\"");
            RestResponse response = _instance.ExecuteAsync(phoneRequest).Result;
            string? phoneContent = response.Content;
            int bpoint = 0;
        }

        static string ExtractToken(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            string pattern = @"""token""\s*:\s*""(?<token>[^""]+)""";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(json);

            if (match.Success)
            {
                return match.Groups["token"].Value; // Возврат найденного токена
            }

            return string.Empty; // Если токен не найден
        }

        static void CleanFromChromeProcesses()
        {
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.Kill();
            }
        }

        static void CleanFromChromeDriverProcesses()
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
        }
    }
}
