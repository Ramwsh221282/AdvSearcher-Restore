using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;

internal interface ICianWebDriverCommand<TCommand>
    where TCommand : ICianWebDriverCommand<TCommand>
{
    Task Execute(IWebDriver driver);
}
