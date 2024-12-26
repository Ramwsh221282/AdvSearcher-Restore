using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPopupElement;

internal sealed class GetPopupElementQuery
    : IWebDriverQuery<GetPopupElementQuery, Result<IWebElement>>
{
    private const string XPath = ".//*[@data-marker='item-popup/popup']";

    public async Task<Result<IWebElement>> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return new Error("No web driver instance created");

        IWebElement? popup = null;
        int popupTries = 0;
        while (popup == null && popupTries <= 50)
        {
            try
            {
                popup = provider.Instance.FindElement(By.XPath(XPath));
            }
            catch
            {
                popupTries++;
            }
        }

        return popup == null
            ? new Error("Can't find popup element")
            : await Task.FromResult(Result<IWebElement>.Success(popup));
    }
}
