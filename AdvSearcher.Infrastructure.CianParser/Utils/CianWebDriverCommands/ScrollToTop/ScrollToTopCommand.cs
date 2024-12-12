using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands.ScrollToTop;

internal sealed class ScrollToTopCommand : ICianWebDriverCommand<ScrollToTopCommand>
{
    private const string Script = "window.scrollTo(0, 0);";

    public async Task Execute(IWebDriver driver)
    {
        driver.ExecuteJavaScript(Script);
        await Task.CompletedTask;
    }
}
