using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerDialogElement;

internal sealed class GetSellerDialogElementQuery(IWebElement popup)
    : IAvitoWebDriverQuery<GetSellerDialogElementQuery, IWebElement>
{
    private readonly IWebElement _popup = popup;
    private const string XPath = "desktop-y382as";

    public async Task<Result<IWebElement>> Execute(IWebDriver driver)
    {
        IWebElement? dialog = _popup.FindElement(By.ClassName(XPath));
        return dialog == null
            ? new Error("Seller dialog element was not found")
            : await Task.FromResult(Result<IWebElement>.Success(dialog));
    }
}
