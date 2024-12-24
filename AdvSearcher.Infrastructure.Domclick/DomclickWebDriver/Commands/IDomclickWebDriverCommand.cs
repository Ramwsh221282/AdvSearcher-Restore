using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver;

internal interface IDomclickWebDriverCommand<TCommand>
    where TCommand : IDomclickWebDriverCommand<TCommand>
{
    Task ExecuteAsync(IWebDriver driver);
}
