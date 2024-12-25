using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.DependencyInjection;

public static class ParserSDK
{
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
        return services;
    }
}
