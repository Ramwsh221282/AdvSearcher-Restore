using System.Security.Principal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public class WebDriverOptionsManager
{
    private ChromeOptions _options;

    public ChromeOptions ChromeOptions => _options;

    // strategies:
    // default - 0
    // normal - 1
    // eager - 2
    // none - 3
    public WebDriverOptionsManager(int strategy = 3)
    {
        _options = new ChromeOptions();
        if (!Enum.IsDefined(typeof(PageLoadStrategy), strategy))
            throw new NotSupportedException();
        _options.PageLoadStrategy = (PageLoadStrategy)strategy;
        _options.AddArgument(GetChromeUserAppDataPath());
        _options.AddArgument("--start-maximized");
        _options.AddArgument("--disable-blink-features=AutomationControlled");
        _options.AddExcludedArguments("excludeSwitches", "enable-automation");
        _options.AddArgument("--disable-web-security");
        _options.AddArgument("--ignore-certificate-errors");
        _options.AddArgument("--no-sandbox");
        _options.AddArgument("--disable-gpu");
        _options.AddArgument("--disable-dev-shm-usage");
        _options.AddArgument("--allow-running-insecure-content");
        _options.AcceptInsecureCertificates = true;
        _options.AddAdditionalOption("useAutomationExtensions", false);
        _options.AddArgument(
            "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36"
        );
    }

    private string GetChromeUserAppDataPath()
    {
        WindowsIdentity? identity = WindowsIdentity.GetCurrent();
        if (identity == null)
            throw new InvalidOperationException("No active windows identity");
        string lastUserName = identity.Name.Split('\\').Last();
        return "user-data-dir=C:\\Users\\"
            + lastUserName
            + "\\AppData\\Local\\Google\\Chrome\\User Data";
    }
}
