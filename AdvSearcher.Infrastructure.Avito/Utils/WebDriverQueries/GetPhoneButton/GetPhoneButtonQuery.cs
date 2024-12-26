using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPhoneButton;

internal sealed class GetPhoneButtonQuery
    : IWebDriverQuery<GetPhoneButtonQuery, Result<IWebElement>>
{
    private const string PhoneButtonCardPath = "//*[@data-marker='item-phone-button/card']";

    public async Task<Result<IWebElement>> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return new Error("No web driver instnace");

        IWebElement phonebutton = provider.Instance.FindElement(By.XPath(PhoneButtonCardPath));
        return phonebutton == null
            ? new Error("Phone button element not fount")
            : await Task.FromResult(Result<IWebElement>.Success(phonebutton));
    }
}
