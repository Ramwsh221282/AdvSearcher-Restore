using AdvSearcher.Parser.SDK.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.Option.Extension;

public class OptionsExtensionDIServices : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<PathManager>()
            .AddTransient<TxtOptionLogger>()
            .AddTransient<IOptionManager, TxtFileOptionManager>();
        return services;
    }
}
