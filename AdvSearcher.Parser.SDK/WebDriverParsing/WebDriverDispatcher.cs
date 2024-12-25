using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverDispatcher
{
    private readonly IServiceScopeFactory _factory;

    public WebDriverDispatcher(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task Resolve<TWebDriverCommand>(
        TWebDriverCommand command,
        WebDriverProvider driverProvider
    )
        where TWebDriverCommand : IWebDriverCommand<TWebDriverCommand>
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IWebDriverCommand<TWebDriverCommand> action =
            provider.GetRequiredService<TWebDriverCommand>();
        await action.ExecuteAsync(driverProvider);
    }

    public async Task<TResult> Resolve<TWebDriverQuery, TResult>(
        TWebDriverQuery query,
        WebDriverProvider driverProvider
    )
        where TWebDriverQuery : IWebDriverQuery<TWebDriverQuery, TResult>
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IWebDriverQuery<TWebDriverQuery, TResult> action =
            provider.GetRequiredService<TWebDriverQuery>();
        return await action.ExecuteAsync(driverProvider);
    }
}
