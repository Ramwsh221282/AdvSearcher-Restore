using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands.ScrollToTop;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser;

internal sealed class CianParsingMethod(ServiceUrl url)
{
    private const string CardsWrapper = "_93444fe79c--wrapper--W0WqH";
    private const string CardComponents = ".//article[@data-name='CardComponent']";

    public async Task<List<CianAdvertisementCard>> BuildAdvertisementCards(
        IWebDriver driver,
        ICianWebDriverCommandDispatcher dispatcher
    )
    {
        await driver.Navigate().GoToUrlAsync(url.Url.Value);
        await dispatcher.Dispatch(new ScrollToBottomCommand(), driver);
        await dispatcher.Dispatch(new ScrollToTopCommand(), driver);

        var wrapper = driver.FindElement(By.ClassName(CardsWrapper));
        if (wrapper == null)
            return [];

        var items = wrapper.FindElements(By.XPath(CardComponents));
        if (items == null)
            return [];

        var cards = new List<CianAdvertisementCard>();
        foreach (var item in items)
        {
            var card = new CianAdvertisementCard(item);
            if (!card.IsHomeowner())
                continue;
            card.InitializeMobilePhone();
            card.InitializeContent();
            card.InitializeSourceUrl();
            card.InitializeId();
            card.InitializeDate();
            card.InitializePhotos();
            cards.Add(card);
        }
        return cards;
    }
}
