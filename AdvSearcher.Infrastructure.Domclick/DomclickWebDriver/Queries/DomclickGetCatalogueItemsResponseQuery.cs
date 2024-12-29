using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries;

internal sealed class DomclickGetCatalogueItemsResponseQuery
    : IWebDriverQuery<DomclickGetCatalogueItemsResponseQuery, string>
{
    public async Task<string> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return string.Empty;
        IWebElement preElement = null!;
        while (preElement == null)
        {
            try
            {
                preElement = provider.Instance.FindElement(By.TagName("pre"));
            }
            catch
            {
                // ignored
            }
        }
        string data = preElement.Text;
        return await Task.FromResult(data);
    }
}
