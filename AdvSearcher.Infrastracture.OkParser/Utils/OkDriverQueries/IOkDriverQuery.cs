using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries;

public interface IOkDriverQuery<TQuery, TResult>
    where TQuery : IOkDriverQuery<TQuery, TResult>
{
    Task<TResult> Execute(IWebDriver driver);
}
