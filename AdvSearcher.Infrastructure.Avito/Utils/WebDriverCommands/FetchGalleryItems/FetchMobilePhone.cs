using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetAllDivElements;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPhoneButton;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPopupElement;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerDialogElement;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerInfoText;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerPhoneNumber;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.IsSellerHomeowner;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;

internal sealed class FetchMobilePhone(AvitoCatalogueItem item)
    : IWebDriverCommand<FetchMobilePhone>
{
    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        int clickTimes = 0;
        int maxClickTimes = 50;
        bool isClicked = false;
        while (clickTimes < maxClickTimes && !isClicked)
        {
            try
            {
                Result<IWebElement> phoneButton = await new GetPhoneButtonQuery().ExecuteAsync(
                    provider
                );
                if (phoneButton.IsFailure)
                    return;
                phoneButton.Value.Click();
                isClicked = true;
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch
            {
                clickTimes++;
            }
        }

        Result<IWebElement> popupElement = await new GetPopupElementQuery().ExecuteAsync(provider);
        if (popupElement.IsFailure)
            return;

        Result<IWebElement> dialog = await new GetSellerDialogElementQuery(
            popupElement.Value
        ).ExecuteAsync(provider);
        if (dialog.IsFailure)
            return;

        Result<IEnumerable<IWebElement>> allDivElements = await new GetAllDivElementsQuery(
            dialog.Value
        ).ExecuteAsync(provider);
        if (allDivElements.IsFailure)
            return;

        Result<bool> isNotHomeowner = await new IsSellerHomeownerQuery(
            allDivElements.Value
        ).ExecuteAsync(provider);
        if (isNotHomeowner.Value)
            return;

        Result<string> sellerInfoText = await new GetSellerInfoTextQuery(
            allDivElements.Value
        ).ExecuteAsync(provider);
        if (sellerInfoText.IsFailure)
            return;
        await new GetSellerPhoneNumberQuery(item, sellerInfoText.Value).ExecuteAsync(provider);
    }
}
