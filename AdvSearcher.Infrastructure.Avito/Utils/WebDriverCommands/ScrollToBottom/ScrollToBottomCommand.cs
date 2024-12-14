using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.ScrollToBottom;

internal sealed class ScrollToBottomCommand : IAvitoWebDriverCommand<ScrollToBottomCommand>
{
    private const string GetCurrentHeightScript =
        "return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";

    private const string ScrollToBottomScript =
        "window.scrollTo(0, document.documentElement.scrollHeight);";

    public async Task Execute(IWebDriver driver)
    {
        var executor = (IJavaScriptExecutor)driver;
        var initialHeight = (long)executor.ExecuteScript(GetCurrentHeightScript);
        while (true)
        {
            driver.ExecuteJavaScript(ScrollToBottomScript);
            await Task.Delay(1000);
            var currentHeigth = (long)executor.ExecuteScript(GetCurrentHeightScript);
            if (IsEndOfPage(ref initialHeight, ref currentHeigth))
                break;
            initialHeight = currentHeigth;
        }
    }

    private static bool IsEndOfPage(ref long initialHeight, ref long currentHeight)
    {
        return currentHeight == initialHeight;
    }
}
