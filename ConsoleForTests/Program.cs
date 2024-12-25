using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace ConsoleForTests
{
    class Program
    {
        static void Main()
        {
            // string result = new DriverManager().SetUpDriver(
            //     new ChromeConfig(),
            //     VersionResolveStrategy.MatchingBrowser
            // );

            // CleanFromChromeProcesses();
            // CleanFromChromeDriverProcesses();
            //
            // const string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            // const string chromeDriverPath =
            //     @"C:\Program Files\Google\Chrome\Application\chromedriver.exe";
            //
            // const string argument = "--remote-debugging-port=8888";
            // var process = Process.Start(chromePath, argument);
            //
            // while (true)
            // {
            //     if (Process.GetProcessesByName("chrome").Length == 0)
            //         continue;
            //     break;
            // }
            //
            // var options = new ChromeOptions();
            // options.DebuggerAddress = "127.0.0.1:8888";
            // IWebDriver driver = new ChromeDriver(
            //     chromeDriverDirectory: chromeDriverPath,
            //     options: options
            // );
            // driver
            //     .Navigate()
            //     .GoToUrl(
            //         "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_0q0MrSqLraysFJKK8rPDUhMT1WyLrYytlLKTSxQsq4FBAAA__8Xe4TEHwAAAA&s=104"
            //     );
        }

        // static void CleanFromChromeProcesses()
        // {
        //     foreach (var process in Process.GetProcessesByName("chrome"))
        //     {
        //         process.Kill();
        //     }
        // }

        // static void CleanFromChromeDriverProcesses()
        // {
        //     foreach (var process in Process.GetProcessesByName("chromedriver"))
        //     {
        //         process.Kill();
        //     }
        // }
    }
}
