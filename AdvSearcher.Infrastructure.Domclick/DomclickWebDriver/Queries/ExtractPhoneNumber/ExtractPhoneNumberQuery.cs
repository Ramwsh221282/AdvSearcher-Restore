using System.Text;
using AdvSearcher.Core.Tools;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries.ExtractPhoneNumber;

internal sealed class ExtractPhoneNumberQuery
    : IDomclickWebDriverQuery<ExtractPhoneNumberQuery, string>
{
    private const string SideBarXPath = "//div[@class='product-page__sidebar']";

    public async Task<Result<string>> ExecuteAsync(IWebDriver driver)
    {
        IWebElement sideBar = ExtractSideBar(driver);
        try
        {
            IWebElement element1 = sideBar.FindElement(
                By.XPath("//div[@class='product-page__sticky']")
            );
            IWebElement element2 = element1
                .FindElement(By.XPath("//div[@class='xBoTt']"))
                .FindElement(By.XPath("//div[@class='SvG_f LfZBz']"));
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                element2.FindElement(By.XPath("//div[@data-e2e-id='agent-card']"));
                return string.Empty;
            }
            catch
            {
                // ignored
            }

            try
            {
                element2.FindElement(By.XPath("//div[@data-e2e-id='telephony-agency-container']"));
                return string.Empty;
            }
            catch
            {
                stringBuilder.Append(element2.Text.Split('\n').Last().Trim()).Append(" ");
            }
            IWebElement element3 = element1
                .FindElement(By.XPath("//div[@class='HSowq']"))
                .FindElement(By.XPath("//div[@class='wBZ72']"));
            for (bool flag = false; !flag; flag = true)
                element3.Click();
            string text;
            do
            {
                text = element3.Text;
            } while (string.IsNullOrWhiteSpace(text) || !text.Any(c => char.IsDigit(c)));
            stringBuilder.AppendLine(text);
            stringBuilder.Replace("\n", " ");
            string phone = stringBuilder.ToString();
            await Task.Delay(TimeSpan.FromSeconds(1));
            if (string.IsNullOrWhiteSpace(phone))
                return new Error("Can't parse mobile phone");
            return await Task.FromResult(phone);
        }
        catch
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return new Error("Can't parse mobile phone");
        }
    }

    private IWebElement ExtractSideBar(IWebDriver driver)
    {
        IWebElement? sideBar = null;
        while (sideBar == null)
        {
            try
            {
                sideBar = driver.FindElement(By.XPath(SideBarXPath));
                break;
            }
            catch
            {
                // ignored
            }
        }
        return sideBar;
    }
}
