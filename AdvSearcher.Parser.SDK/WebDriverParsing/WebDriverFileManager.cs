using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverFileManager
{
    public string OriginalChromePath { get; set; } = string.Empty;
    public string WebDriverPath { get; private set; } = string.Empty;

    public bool IsChromeInstalled => File.Exists(OriginalChromePath);
    public bool IsWebDriverInstalled => File.Exists(WebDriverPath);

    public WebDriverFileManager() => SetWebDriverPathMOK();

    public void DownloadAndSetupChromeDriver()
    {
        WebDriverPath = new DriverManager().SetUpDriver(
            new ChromeConfig(),
            VersionResolveStrategy.MatchingBrowser
        );
    }

    // TODO: Remove method once web driver binaries management created.
    private void SetWebDriverPathMOK()
    {
        OriginalChromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
    }
}
