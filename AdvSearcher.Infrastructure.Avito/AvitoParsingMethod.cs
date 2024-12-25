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
        await Scroll(driver);
        var html = await ExtractHtml(driver);
        if (html.IsFailure)
            return html.Error;
        var items = CreateItems(html.Value);
        if (items.IsFailure)
            return items.Error;
        return FilterFromIncorrect(items);
    }

    private async Task Scroll(IWebDriver driver)
    {
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
    }

    private async Task<Result<string>> ExtractHtml(IWebDriver driver) =>
        await dispatcher.HandleQuery<ExtractHtmlQuery, string>(new ExtractHtmlQuery(), driver);

    private Result<AvitoCatalogueItem[]> CreateItems(string html)
    {
        var catalogue = new AvitoCatalogue(html);
        return AvitoCatalogueItem.CreateFromCatalogue(catalogue);
    }

    private List<AvitoCatalogueItem> FilterFromIncorrect(AvitoCatalogueItem[] items) =>
        items.Where(i => i.IsCorrect).ToList();
}
