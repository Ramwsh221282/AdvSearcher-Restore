using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

public sealed class ScrollToBottomCommand : IWebDriverCommand
{
    private const string GetCurrentHeightScript =
        "return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";

    private const string ScrollToBottomScript =
        "window.scrollTo(0, document.documentElement.scrollHeight);";

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;

        var executor = (IJavaScriptExecutor)provider.Instance;
        try
        {
            bool isScrolled = false;
            long initialHeight = (long)executor.ExecuteScript(GetCurrentHeightScript);
            while (!isScrolled)
            {
                while (true)
                {
                    provider.Instance.ExecuteJavaScript(ScrollToBottomScript);
                    await Task.Delay(1000);
                    var currentHeigth = (long)executor.ExecuteScript(GetCurrentHeightScript);
                    if (IsEndOfPage(ref initialHeight, ref currentHeigth))
                    {
                        isScrolled = true;
                        break;
                    }
                    initialHeight = currentHeigth;
                }
            }
        }
        catch
        {
            // ignored
        }
    }

    private static bool IsEndOfPage(ref long initialHeight, ref long currentHeight)
    {
        return currentHeight == initialHeight;
    }
}
