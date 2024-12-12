using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;

public interface ICianWebDriverCommand<TCommand>
    where TCommand : ICianWebDriverCommand<TCommand>
{
    Task Execute(IWebDriver driver);
}
