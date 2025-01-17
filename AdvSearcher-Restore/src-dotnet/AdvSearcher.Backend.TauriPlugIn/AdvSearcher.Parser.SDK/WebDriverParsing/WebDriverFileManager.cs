using AdvSearcher.Parser.SDK.Options;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverFileManager
{
    private const string ChromeDriverSettings = "ChromeDriver_Settings.txt";
    private const string ChromeDriverPath = "ChromeDriverPath";

    public string OriginalChromePath { get; private set; } = string.Empty;
    public string WebDriverPath { get; private set; } = string.Empty;

    public bool IsChromeInstalled => File.Exists(OriginalChromePath);
    public bool IsWebDriverInstalled => File.Exists(WebDriverPath);

    public WebDriverFileManager(IOptionManager manager)
    {
        ReadOriginalChromeSettings(manager);
        InitializeChromeDriverPath(manager);
        Console.WriteLine($"OriginalChromePath: {OriginalChromePath}");
        Console.WriteLine($"WebDriverPath: {WebDriverPath}");
    }

    private void ReadOriginalChromeSettings(IOptionManager manager)
    {
        IOptionProcessor reader = manager.CreateReader("WebDriver_Settings.txt");
        Option option = reader
            .Process(new Option("Путь к браузеру Google Chrome", string.Empty))
            .Result;
        if (string.IsNullOrEmpty(option.Value))
            OriginalChromePath = string.Empty;
        OriginalChromePath = Path.Combine(option.Value, "chrome.exe");
    }

    private void InitializeChromeDriverPath(IOptionManager manager)
    {
        IOptionProcessor reader = manager.CreateReader(ChromeDriverSettings);
        Option option = reader.Process(new Option(ChromeDriverPath, string.Empty)).Result;
        if (string.IsNullOrWhiteSpace(option.Value))
        {
            LoadChromeDriver(manager);
            return;
        }
        WebDriverPath = option.Value;
    }

    private void LoadChromeDriver(IOptionManager manager)
    {
        IOptionProcessor writer = manager.CreateWrite(ChromeDriverSettings);
        WebDriverPath = new DriverManager().SetUpDriver(
            new ChromeConfig(),
            VersionResolveStrategy.MatchingBrowser
        );
        Option option = writer.Process(new Option(ChromeDriverPath, WebDriverPath)).Result;
        WebDriverPath = option.Value;
    }
}
