using System.Reflection;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.DependencyInjection;

public static class ParserSDK
{
    private static readonly string Path =
        $@"{AppDomain.CurrentDomain.BaseDirectory}\Plugins\Parsers";

    public static IServiceCollection AddParserSDK(this IServiceCollection services)
    {
        services = services
            .AddTransient<IHttpClient, RestHttpClient>()
            .AddTransient<IHttpService, HttpService>()
            .AddTransient<WebDriverCommandsPipeline>()
            .AddTransient<WebDriverProvider>()
            .AddTransient<WebDriverDispatcher>()
            .AddTransient<IWebDriverCommand<ScrollToBottomCommand>, ScrollToBottomCommand>()
            .AddTransient<IWebDriverCommand<NavigateOnPageCommand>, NavigateOnPageCommand>()
            .AddTransient<IWebDriverCommand<ScrollToTopCommand>, ScrollToTopCommand>();
        //services = services.AddParserPlugins();
        return services;
    }

    public static IServiceCollection AddParserPlugins(this IServiceCollection services)
    {
        if (!Directory.Exists(Path))
            return services;

        Assembly[] assemblies = Directory
            .GetFiles(Path, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        services = services
            .Scan(x =>
                x.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IParser)))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
            )
            .AddTransient<ParserProvider>(p =>
            {
                IEnumerable<IParser> parsers = p.GetServices<IParser>();
                return new ParserProvider(parsers);
            });
        return services;
    }
}
