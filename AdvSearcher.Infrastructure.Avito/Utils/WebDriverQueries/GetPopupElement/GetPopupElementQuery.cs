using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPopupElement;

internal sealed class GetPopupElementQuery : IAvitoWebDriverQuery<GetPopupElementQuery, IWebElement>
{
    private const string XPath = ".//*[@data-marker='item-popup/popup']";

    public async Task<Result<IWebElement>> Execute(IWebDriver driver)
    {
        IWebElement? popup = null;
        int popupTries = 0;
        while (popup == null && popupTries <= 50)
        {
            try
            {
                popup = driver.FindElement(By.XPath(XPath));
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
