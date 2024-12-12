using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries.ExtractHtml;
using AdvSearcher.Infrastracture.OkParser.Utils.OkWebDriver;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkParsingMethod(IWebDriver driver, IOkWebDriverDispatcher dispatcher)
{
    public async Task<string?> ExtractPageHtml(ServiceUrl url)
    {
        await driver.Navigate().GoToUrlAsync(url.Url.Value);
        await dispatcher.Dispatch(new ScrollToBottomCommand(), driver);
        var html = await dispatcher.Dispatch<ExtractHtmlQuery, string>(
            new ExtractHtmlQuery(),
            driver
        );
        return html;
    }
}
