using System.Diagnostics;
using System.Security.Principal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

internal sealed class AvitoWebDriverProvider
{
    private const string UserAgentOption =
        "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";

    private const string MaximizedOption = "--start-maximized";
    private const string AutomationControlledOption =
        "--disable-blink-features=AutomationControlled";

    public IWebDriver BuildWebDriver()
    {
        CleanFromChromeProcesses();
        if (!CheckForChromeDriver())
            DownloadChromeDriver();

        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        var options = new ChromeOptions();
        options.PageLoadStrategy = PageLoadStrategy.None;
        var userName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
        options.AddArgument(
            $@"user-data-dir=C:\Users\{userName}\AppData\Local\Google\Chrome\User Data"
        );
        options.AddArgument(MaximizedOption);
        options.AddArgument(AutomationControlledOption);
        options.AddExcludedArguments("excludeSwitches", "enable-automation");
        options.AddAdditionalOption("useAutomationExtensions", false);
        options.AddArgument(UserAgentOption);
        return new ChromeDriver(service, options);
    }

    private static void CleanFromChromeProcesses()
    {
        foreach (var process in Process.GetProcessesByName("chrome"))
        {
            process.Kill();
        }

        foreach (var process in Process.GetProcessesByName("chromedriver"))
        {
            process.Kill();
        }
    }

    private static bool CheckForChromeDriver() => Directory.Exists("chrome");

    private static void DownloadChromeDriver()
    {
        var manager = new DriverManager();
        manager.SetUpDriver(new ChromeConfig());
    }
}
