using OpenQA.Selenium.Support.Extensions;

namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;

public sealed class ScrollToTopCommand : WebDriverCommand, IWebDriverCommand<ScrollToTopCommand>
{
    private const string Script = "window.scrollTo(0, 0);";

    public override async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        provider.Instance.ExecuteJavaScript(Script);
        await Task.CompletedTask;
    }
}
