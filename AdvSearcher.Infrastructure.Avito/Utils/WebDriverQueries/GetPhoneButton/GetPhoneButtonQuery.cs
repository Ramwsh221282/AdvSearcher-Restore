using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetPhoneButton;

internal sealed class GetPhoneButtonQuery : IAvitoWebDriverQuery<GetPhoneButtonQuery, IWebElement>
{
    private const string PhoneButtonCardPath = "//*[@data-marker='item-phone-button/card']";

    public async Task<Result<IWebElement>> Execute(IWebDriver driver)
    {
        IWebElement phonebutton = driver.FindElement(By.XPath(PhoneButtonCardPath));
        return phonebutton == null
            ? new Error("Phone button element not fount")
            : await Task.FromResult(Result<IWebElement>.Success(phonebutton));
    }
}
