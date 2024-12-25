using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public class WebDriverProvider : IDisposable
{
    private readonly WebDriverFileManager _fileManager;
    private readonly WebDriverOptionsManager _options;

    private Process? _process;
    private IWebDriver? _webDriver;
    public IWebDriver? Instance => _webDriver;

    public WebDriverProvider()
    {
        _fileManager = new WebDriverFileManager();
        _options = new WebDriverOptionsManager();
    }

    public void InstantiateNewWebDriver()
    {
        CleanFromChromeProcesses();
        CleanFromChromeDriverProcesses();
        InstantiateNewChromeProcess();
        _webDriver = new ChromeDriver(
            options: _options.ChromeOptions,
            chromeDriverDirectory: _fileManager.WebDriverPath
        );
    }

    public void Dispose()
    {
        _process?.Kill();
        _webDriver?.Dispose();
    }

    private void InstantiateNewChromeProcess()
    {
        const string argument = "--remote-debugging-port=8888";
        _process = Process.Start(_fileManager.OriginalChromePath, argument);
        WaitForProcessStart();
    }

    private void WaitForProcessStart()
    {
        while (true)
        {
            if (Process.GetProcessesByName("chrome").Length == 0)
                continue;
            break;
        }
    }

    private void CleanFromChromeProcesses()
    {
        foreach (var process in Process.GetProcessesByName("chrome"))
        {
            process.Kill();
        }
    }

    private void CleanFromChromeDriverProcesses()
    {
        foreach (var process in Process.GetProcessesByName("chromedriver"))
        {
            process.Kill();
        }
    }
}
