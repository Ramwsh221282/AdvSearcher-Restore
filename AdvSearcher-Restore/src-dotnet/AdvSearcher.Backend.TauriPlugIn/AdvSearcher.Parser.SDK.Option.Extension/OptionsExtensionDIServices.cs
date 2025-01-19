using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Parser.SDK.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.Option.Extension;

public sealed class OptionsExtensionDIServices : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public OptionsExtensionDIServices(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<PathManager>()
            .AddScoped<TxtOptionLogger>()
            .AddScoped<IOptionManager, TxtFileOptionManager>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        _listener.Publish("Options plugin loaded.");
        return services;
    }

    public void SetMessageListener(IMessageListener listener)
    {
        _listener = listener;
    }
}
