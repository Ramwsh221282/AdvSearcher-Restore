using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;

internal sealed class CianWebDriverCommandDispatcher(IServiceScopeFactory factory)
    : ICianWebDriverCommandDispatcher
{
    public async Task Dispatch<TCommand>(TCommand command, IWebDriver driver)
        where TCommand : ICianWebDriverCommand<TCommand>
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;
        var handler = provider.GetRequiredService<ICianWebDriverCommand<TCommand>>();
        await handler.Execute(driver);
    }
}
