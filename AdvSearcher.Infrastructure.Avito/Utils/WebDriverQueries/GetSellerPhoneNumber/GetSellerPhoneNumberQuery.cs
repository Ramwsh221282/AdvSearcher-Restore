using System.Text;
using System.Text.RegularExpressions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetSellerPhoneNumber;

internal sealed class GetSellerPhoneNumberQuery
    : IAvitoWebDriverQuery<GetSellerPhoneNumberQuery, string>
{
    private const string MainPhoneGetUrl = "https://m.avito.ru/api/1/items/";
    private const string SecretKey = "/phone?key=af0deccbgcgidddjgnvljitntccdduijhdinfgjgfjir";
    private const string Tag = "pre";
    private readonly AvitoCatalogueItem _item;
    private readonly string _sellerInfoText;

    public GetSellerPhoneNumberQuery(AvitoCatalogueItem item, string sellerInfoText)
    {
        _item = item;
        _sellerInfoText = sellerInfoText;
    }

    public async Task<Result<string>> Execute(IWebDriver driver)
    {
        await NavigateToPhonePage(driver);
        IWebElement jsonInfoContainer = GetJsonInfoContainer(driver);
        var match = Regex.Match(
            jsonInfoContainer.GetAttribute("innerHTML"),
            "\"Позвонить на\\s*(.*?)\""
        );
        if (match.Success)
        {
            var names = Regex.Replace(match.Groups[1].Value, "\\D", "");
            var avitoPublisherInfoBuilder = new StringBuilder();
            avitoPublisherInfoBuilder.AppendLine(_sellerInfoText);
            avitoPublisherInfoBuilder.AppendLine(names);
            _item.PublisherInfo = avitoPublisherInfoBuilder.ToString();
            _item.IsHomeOwner = true;
            return _item.PublisherInfo;
        }
        return new Error("Can't get seller info phone number");
    }

    private async Task NavigateToPhonePage(IWebDriver driver)
    {
        StringBuilder phoneNavigationLinkBuilder = new StringBuilder();
        phoneNavigationLinkBuilder.Append(MainPhoneGetUrl);
        phoneNavigationLinkBuilder.Append(_item.Id);
        phoneNavigationLinkBuilder.Append(SecretKey);
        await driver.Navigate().GoToUrlAsync(phoneNavigationLinkBuilder.ToString());
    }

    private IWebElement GetJsonInfoContainer(IWebDriver driver)
    {
        IWebElement jsonInfoContainer = null;
        while (jsonInfoContainer == null)
        {
            try
            {
                jsonInfoContainer = driver.FindElement(By.TagName(Tag));
            }
            catch
            {
                // ignored
            }
        }

        return jsonInfoContainer;
    }
}
