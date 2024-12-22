using AdvSearcher.Core.Tools;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

internal interface IAvitoWebDriverQuery<TQuery, TResult>
{
    Task<Result<TResult>> Execute(IWebDriver driver);
}
