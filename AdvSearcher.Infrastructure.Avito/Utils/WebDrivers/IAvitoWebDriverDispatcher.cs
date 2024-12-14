using AdvSearcher.Core.Tools;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

public interface IAvitoWebDriverDispatcher
{
    Task HandleCommand<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : IAvitoWebDriverCommand<TCommand>;

    Task<Result<TResult>> HandleQuery<TQuery, TResult>(TQuery query, IWebDriver driver)
        where TQuery : IAvitoWebDriverQuery<TQuery, TResult>;
}
