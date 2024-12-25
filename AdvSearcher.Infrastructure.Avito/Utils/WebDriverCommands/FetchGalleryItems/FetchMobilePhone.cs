using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetAllDivElements;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPhoneButton;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPopupElement;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerDialogElement;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerInfoText;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerPhoneNumber;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.IsSellerHomeowner;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;

internal sealed class FetchMobilePhone(AvitoCatalogueItem item)
    : IAvitoWebDriverCommand<FetchMobilePhone>
{
    public async Task Execute(IWebDriver driver)
    {
        Result<IWebElement> phoneButton = await new GetPhoneButtonQuery().Execute(driver);
        if (phoneButton.IsFailure)
            return;
        phoneButton.Value.Click();
        await Task.Delay(TimeSpan.FromSeconds(1));

        Result<IWebElement> popupElement = await new GetPopupElementQuery().Execute(driver);
        if (popupElement.IsFailure)
            return;

        Result<IWebElement> dialog = await new GetSellerDialogElementQuery(
            popupElement.Value
        ).Execute(driver);
        if (dialog.IsFailure)
            return;

        Result<IEnumerable<IWebElement>> allDivElements = await new GetAllDivElementsQuery(
            dialog.Value
        ).Execute(driver);
        if (allDivElements.IsFailure)
            return;

        Result<bool> isNotHomeowner = await new IsSellerHomeownerQuery(
            allDivElements.Value
        ).Execute(driver);
        if (isNotHomeowner.Value)
            return;

        Result<string> sellerInfoText = await new GetSellerInfoTextQuery(
            allDivElements.Value
        ).Execute(driver);
        if (sellerInfoText.IsFailure)
            return;

        await new GetSellerPhoneNumberQuery(item, sellerInfoText.Value).Execute(driver);
    }
}
