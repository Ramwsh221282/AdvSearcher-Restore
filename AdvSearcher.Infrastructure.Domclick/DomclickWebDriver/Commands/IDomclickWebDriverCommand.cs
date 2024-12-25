using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Commands;

internal interface IDomclickWebDriverCommand<TCommand>
    where TCommand : IDomclickWebDriverCommand<TCommand>
{
    Task ExecuteAsync(IWebDriver driver);
}
