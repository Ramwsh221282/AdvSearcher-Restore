using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.ExtractHtml;

internal sealed class ExtractHtmlQuery : IAvitoWebDriverQuery<ExtractHtmlQuery, string>
{
    public async Task<Result<string>> Execute(IWebDriver driver)
    {
        var html = driver.PageSource;
        if (string.IsNullOrWhiteSpace(html))
            return ParserErrors.HtmlEmpty;
        return await Task.FromResult(html);
    }
}
