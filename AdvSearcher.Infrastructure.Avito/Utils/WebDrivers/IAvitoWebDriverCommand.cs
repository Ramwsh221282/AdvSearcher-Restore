using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

internal interface IAvitoWebDriverCommand<TCommand>
{
    Task Execute(IWebDriver driver);
}
