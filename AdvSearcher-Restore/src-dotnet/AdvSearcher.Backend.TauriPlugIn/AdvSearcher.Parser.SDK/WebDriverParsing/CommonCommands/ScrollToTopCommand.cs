using OpenQA.Selenium.Support.Extensions;

namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

public sealed class ScrollToTopCommand : IWebDriverCommand
{
    private const string Script = "window.scrollTo(0, 0);";

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        provider.Instance.ExecuteJavaScript(Script);
        await Task.CompletedTask;
    }
}
