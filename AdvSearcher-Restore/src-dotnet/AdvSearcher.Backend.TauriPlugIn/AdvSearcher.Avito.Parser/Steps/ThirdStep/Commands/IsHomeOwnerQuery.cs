using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Avito.Parser.Steps.ThirdStep.Commands;

public sealed class IsHomeOwnerQuery : IWebDriverQuery<bool>
{
    private const string SellerInfoXPath = ".//div[@data-marker='item-view/seller-info']";

    public async Task<bool> ExecuteAsync(WebDriverProvider provider)
    {
        IWebElement sellerInfoElement = null!;
        while (sellerInfoElement == null)
        {
            try
            {
                sellerInfoElement = provider.Instance.FindElement(By.XPath(SellerInfoXPath));
            }
            catch
            {
                // ignored
            }
        }
        string sellerInfoText = sellerInfoElement.Text;
        if (sellerInfoText.Contains("Частное лицо", StringComparison.OrdinalIgnoreCase))
            return await Task.FromResult(true);
        return await Task.FromResult(false);
    }
}
