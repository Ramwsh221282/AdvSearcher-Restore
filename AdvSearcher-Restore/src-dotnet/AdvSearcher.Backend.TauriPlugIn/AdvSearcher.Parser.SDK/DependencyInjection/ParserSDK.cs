using System.Reflection;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.DependencyInjection;

public static class ParserSDK
{
    private static readonly string Path = $@"{Environment.CurrentDirectory}\Plugins\Parsers";

    public static IServiceCollection AddParserSDK(this IServiceCollection services)
    {
        services = services
            .AddTransient<IHttpClient, RestHttpClient>()
            .AddTransient<IHttpService, HttpService>()
            .AddScoped<WebDriverProvider>()
            .AddSingleton<ParserResolver>();
        services = services.AddParserPlugins();
        return services;
    }

    private static IServiceCollection AddParserPlugins(this IServiceCollection services)
    {
        if (!Directory.Exists(Path))
            return services;

        Assembly[] assemblies = Directory
            .GetFiles(Path, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IParserDiServicesInitializer>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );
        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IParserDiServicesInitializer> initializers =
            provider.GetServices<IParserDiServicesInitializer>();

        if (!initializers.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR. Parser plugins were not loaded");
            throw new ApplicationException();
        }

        foreach (IParserDiServicesInitializer initializer in initializers)
        {
            initializer.ModifyServices(services);
        }
        return services;
    }
}
