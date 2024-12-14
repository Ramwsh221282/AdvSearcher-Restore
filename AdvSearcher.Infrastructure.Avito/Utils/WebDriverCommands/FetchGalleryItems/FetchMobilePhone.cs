using System.Text;
using System.Text.RegularExpressions;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;

internal sealed class FetchMobilePhone(AvitoCatalogueItem item)
    : IAvitoWebDriverCommand<FetchMobilePhone>
{
    private const string PhoneButtonCardPath = "//*[@data-marker='item-phone-button/card']";
    private const string SellerInfoPath = "//*[@data-marker='item-view/seller-info']";
    private const string SellerLinkPath = "//*[@data-marker='seller-link/link']";
    private const string ContactsPopup = ".//*[@data-marker='item-popup/popup']";
    private const string MainPhoneGetUrl = "https://m.avito.ru/api/1/items/";
    private const string SecretKey = "/phone?key=af0deccbgcgidddjgnvljitntccdduijhdinfgjgfjir";
    private const string PopUpInnerClass = "desktop-y382as";

    public async Task Execute(IWebDriver driver)
    {
        var phoneButtonCardElement = driver.FindElement(By.XPath(PhoneButtonCardPath));
        phoneButtonCardElement.Click();
        await Task.Delay(1000);
        IWebElement popupElement = null;
        int popupTries = 0;
        while (popupElement == null && popupTries <= 50)
        {
            try
            {
                popupElement = driver.FindElement(By.XPath(ContactsPopup));
            }
            catch
            {
                popupTries++;
            }
        }

        var dialog = popupElement.FindElement(By.ClassName(PopUpInnerClass));

        var allDivElements = dialog.FindElements(By.TagName("div"));
        var elementWithHomeOwnerTag = allDivElements.FirstOrDefault(el =>
            el.Text.Contains("Частное лицо", StringComparison.OrdinalIgnoreCase)
        );
        if (elementWithHomeOwnerTag == null)
            return;

        var sellerInfoText = string.Empty;
        foreach (var divElement in allDivElements)
        {
            var sellerInfoElement = divElement.FindElement(
                By.XPath("//*[@data-marker='seller-info-new-design']")
            );
            if (sellerInfoElement == null)
                continue;
            sellerInfoText = sellerInfoElement.Text;
        }

        if (string.IsNullOrWhiteSpace(sellerInfoText))
            return;

        var phoneNavigationLinkBuilder = new StringBuilder();
        phoneNavigationLinkBuilder.Append(MainPhoneGetUrl);
        phoneNavigationLinkBuilder.Append(item.Id);
        phoneNavigationLinkBuilder.Append(SecretKey);
        await driver.Navigate().GoToUrlAsync(phoneNavigationLinkBuilder.ToString());
        IWebElement jsonInfoContainer = null;
        while (jsonInfoContainer == null)
        {
            try
            {
                jsonInfoContainer = driver.FindElement(By.TagName("pre"));
            }
            catch
            {
                // ignored
            }
        }
        var match = Regex.Match(
            jsonInfoContainer.GetAttribute("innerHTML"),
            "\"Позвонить на\\s*(.*?)\""
        );
        if (match.Success)
        {
            var names = Regex.Replace(match.Groups[1].Value, "\\D", "");
            var avitoPublisherInfoBuilder = new StringBuilder();
            avitoPublisherInfoBuilder.AppendLine(sellerInfoText);
            avitoPublisherInfoBuilder.AppendLine(names);
            item.PublisherInfo = avitoPublisherInfoBuilder.ToString();
            item.IsHomeOwner = true;
        }
    }
}
