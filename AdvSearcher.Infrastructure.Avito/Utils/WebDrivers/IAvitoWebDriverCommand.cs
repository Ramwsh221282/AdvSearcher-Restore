using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

public interface IAvitoWebDriverCommand<TCommand>
{
    Task Execute(IWebDriver driver);
}
