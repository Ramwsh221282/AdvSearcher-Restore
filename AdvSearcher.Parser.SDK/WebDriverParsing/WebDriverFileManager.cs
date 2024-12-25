using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverFileManager
{
    public string OriginalChromePath { get; init; }
    public string WebDriverPath { get; private set; } = string.Empty;

    public bool IsChromeInstalled => File.Exists(OriginalChromePath);
    public bool IsWebDriverInstalled => File.Exists(WebDriverPath);

    public WebDriverFileManager(
        string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
    )
    {
        OriginalChromePath = chromePath;
        SetWebDriverPathMOK();
    }

    public void DownloadAndSetupChromeDriver()
    {
        WebDriverPath = new DriverManager().SetUpDriver(
            new ChromeConfig(),
            VersionResolveStrategy.MatchingBrowser
        );
    }

    private void SetWebDriverPathMOK()
    {
        WebDriverPath = @"C:\Program Files\Google\Chrome\Application\chromedriver.exe";
    }
}
