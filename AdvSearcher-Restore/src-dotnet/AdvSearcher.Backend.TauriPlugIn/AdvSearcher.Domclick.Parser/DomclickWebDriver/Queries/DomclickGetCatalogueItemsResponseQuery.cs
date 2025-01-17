using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Domclick.Parser.DomclickWebDriver.Queries;

public sealed class DomclickGetCatalogueItemsResponseQuery : IWebDriverQuery<string>
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
