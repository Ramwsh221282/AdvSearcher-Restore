using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverOptionsManager
{
    private ChromeOptions _options;

    public ChromeOptions ChromeOptions => _options;

    public WebDriverOptionsManager()
    {
        _options = new ChromeOptions();
        _options.PageLoadStrategy = PageLoadStrategy.None;
        _options.DebuggerAddress = "127.0.0.1:8888";
    }
}
