using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerInfoText;

internal sealed class GetSellerInfoTextQuery : IAvitoWebDriverQuery<GetSellerInfoTextQuery, string>
{
    private readonly IEnumerable<IWebElement> _elements;
    private const string XPath = "//*[@data-marker='seller-info-new-design']";

    public GetSellerInfoTextQuery(IEnumerable<IWebElement> elements) => _elements = elements;

    public async Task<Result<string>> Execute(IWebDriver driver)
    {
        foreach (var element in _elements)
        {
            IWebElement? sellerInfoElement = element.FindElement(By.XPath(XPath));
            if (sellerInfoElement == null)
                continue;
            return await Task.FromResult(sellerInfoElement.Text);
        }

        return await Task.FromResult(new Error("Can't find seller info"));
    }
}
