using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public class WebDriverProvider : IDisposable
{
    public WebDriverFileManager FileManager { get; }
    public WebDriverOptionsManager Options { get; }

    private IWebDriver? _webDriver;
    public IWebDriver? Instance => _webDriver;

    public WebDriverProvider()
    {
        FileManager = new WebDriverFileManager();
        Options = new WebDriverOptionsManager();
    }

    public void InstantiateNewWebDriver()
    {
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        CleanFromChromeProcesses();
        CleanFromChromeDriverProcesses();
        _webDriver = new ChromeDriver(service, Options.ChromeOptions);
    }

    public void Dispose() => _webDriver?.Dispose();

    private void CleanFromChromeProcesses()
    {
        Process[] processes = Process.GetProcessesByName("chrome");
        if (!processes.Any())
            return;
        foreach (var process in Process.GetProcessesByName("chrome"))
        {
            process.Kill();
        }
    }

    private void CleanFromChromeDriverProcesses()
    {
        Process[] processes = Process.GetProcessesByName("chromedriver");
        if (!processes.Any())
            return;
        foreach (var process in Process.GetProcessesByName("chromedriver"))
        {
            process.Kill();
        }
    }
}
