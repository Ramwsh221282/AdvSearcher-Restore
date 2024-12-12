using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;

public interface ICianWebDriverCommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : ICianWebDriverCommand<TCommand>;
}
