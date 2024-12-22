using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkWebDriver;

internal interface IOkWebDriverDispatcher
{
    Task Dispatch<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : IOkDriverCommand<TCommand>;

    Task<TResult> Dispatch<TQuery, TResult>(TQuery query, IWebDriver driver)
        where TQuery : IOkDriverQuery<TQuery, TResult>;
}
