using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries.ExtractHtml;

internal sealed class ExtractHtmlQuery : IOkDriverQuery<ExtractHtmlQuery, string>
{
    public async Task<string> Execute(IWebDriver driver)
    {
        var html = driver.PageSource;
        return await Task.FromResult(html);
    }
}
