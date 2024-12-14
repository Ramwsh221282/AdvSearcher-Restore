using AdvSearcher.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

public class AvitoWebDriverDispatcher(IServiceScopeFactory factory) : IAvitoWebDriverDispatcher
{
    public async Task HandleCommand<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : IAvitoWebDriverCommand<TCommand>
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;
        var requestedCommand = provider.GetRequiredService<IAvitoWebDriverCommand<TCommand>>();
        await requestedCommand.Execute(driver);
    }

    public async Task<Result<TResult>> HandleQuery<TQuery, TResult>(TQuery query, IWebDriver driver)
        where TQuery : IAvitoWebDriverQuery<TQuery, TResult>
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;
        var requestedQuery = provider.GetRequiredService<IAvitoWebDriverQuery<TQuery, TResult>>();
        return await requestedQuery.Execute(driver);
    }
}
