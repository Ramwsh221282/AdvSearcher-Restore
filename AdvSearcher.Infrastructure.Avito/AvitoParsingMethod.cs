using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.ExtractHtml;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoParsingMethod(IAvitoWebDriverDispatcher dispatcher)
{
    public async Task<Result<List<AvitoCatalogueItem>>> ExecuteMethod(
        IWebDriver driver,
        ServiceUrl url
    )
    {
        await driver.Navigate().GoToUrlAsync(url.Url.Value);
        bool isScrolled = false;
        while (!isScrolled)
        {
            try
            {
                await dispatcher.HandleCommand(new ScrollToBottomCommand(), driver);
                isScrolled = true;
            }
            catch
            {
                // ignored
            }
        }
        var html = await dispatcher.HandleQuery<ExtractHtmlQuery, string>(
            new ExtractHtmlQuery(),
            driver
        );
        if (html.IsFailure)
            return html.Error;
        var catalogue = new AvitoCatalogue(html.Value);
        var items = AvitoCatalogueItem.CreateFromCatalogue(catalogue);
        if (items.IsFailure)
            return items.Error;
        return items.Value.Where(i => i.IsCorrect).ToList();
    }
}
