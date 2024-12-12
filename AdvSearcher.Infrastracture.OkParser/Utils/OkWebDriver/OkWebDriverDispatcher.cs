using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkWebDriver;

internal sealed class OkWebDriverDispatcher(IServiceScopeFactory factory) : IOkWebDriverDispatcher
{
    public async Task Dispatch<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : IOkDriverCommand<TCommand>
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;
        var executor = provider.GetRequiredService<IOkDriverCommand<TCommand>>();
        await executor.ExecuteAsync(driver);
    }

    public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query, IWebDriver driver)
        where TQuery : IOkDriverQuery<TQuery, TResult>
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;
        var executor = provider.GetRequiredService<IOkDriverQuery<TQuery, TResult>>();
        var result = await executor.Execute(driver);
        return result;
    }
}
