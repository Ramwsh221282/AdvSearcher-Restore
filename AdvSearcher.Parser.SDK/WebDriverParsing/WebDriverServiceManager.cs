using OpenQA.Selenium.Chrome;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverServiceManager
{
    private readonly ChromeDriverService _service;

    public ChromeDriverService Service => _service;

    public WebDriverServiceManager()
    {
        _service = ChromeDriverService.CreateDefaultService();
        _service.HideCommandPromptWindow = true;
    }
}
