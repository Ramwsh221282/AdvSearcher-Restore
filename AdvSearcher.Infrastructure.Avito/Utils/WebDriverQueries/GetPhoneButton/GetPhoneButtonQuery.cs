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

        int tries = 100;
        int current = 0;
        IWebElement phoneButton = null!;
        while (current < tries && phoneButton == null)
        {
            try
            {
                phoneButton = provider.Instance.FindElement(By.XPath(PhoneButtonCardPath));
            }
            catch
            {
                current++;
            }
        }

        return phoneButton == null
            ? new Error("Phone button element not fount")
            : await Task.FromResult(Result<IWebElement>.Success(phoneButton));
    }
}
