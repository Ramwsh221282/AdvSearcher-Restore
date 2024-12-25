using AdvSearcher.Core.Tools;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries;

internal interface IDomclickWebDriverQuery<TQuery, TResult>
    where TQuery : IDomclickWebDriverQuery<TQuery, TResult>
{
    Task<Result<TResult>> ExecuteAsync(IWebDriver driver);
}
