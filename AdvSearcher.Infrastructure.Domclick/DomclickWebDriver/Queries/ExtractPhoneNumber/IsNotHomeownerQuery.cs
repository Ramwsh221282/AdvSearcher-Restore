using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries.ExtractPhoneNumber;

internal sealed class IsNotHomeownerQuery : IWebDriverQuery<IsNotHomeownerQuery, bool>
{
    private const string SideBarXPath = "//div[@class='product-page__sidebar']";
    private const string SellerContainerXpath = ".//div[@data-e2e-id='seller-container']";

    public async Task<bool> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return false;

        IWebElement sideBar = ExtractSideBar(provider.Instance);
        IWebElement element1 = sideBar.FindElement(
            By.XPath("//div[@class='product-page__sticky']")
        );
        IWebElement element2 = element1
            .FindElement(By.XPath("//div[@class='xBoTt']"))
            .FindElement(By.XPath("//div[@class='SvG_f LfZBz']"));
        if (HasHomeOwnerTag(sideBar))
            return await Task.FromResult(true);
        if (!HasAgentCard(element2))
            return await Task.FromResult(false);
        if (!HasAgentName(element2))
            return await Task.FromResult(false);
        return await Task.FromResult(true);
    }

    private bool HasHomeOwnerTag(IWebElement sideBar)
    {
        try
        {
            IWebElement homeOwnerTag = sideBar.FindElement(By.XPath(SellerContainerXpath));
            if (homeOwnerTag == null)
                return false;
            string text = homeOwnerTag.Text;
            if (string.IsNullOrEmpty(text))
                return false;
            return text.Contains("Собственник", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private bool HasAgentCard(IWebElement sidebar)
    {
        try
        {
            sidebar.FindElement(By.XPath("//div[@data-e2e-id='agent-card']"));
            return false;
        }
        catch
        {
            return true;
        }
    }

    private bool HasAgentName(IWebElement sidebar)
    {
        try
        {
            sidebar.FindElement(By.XPath("//div[@data-e2e-id='telephony-agency-container']"));
            return false;
        }
        catch
        {
            return true;
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
