using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.VK.Parser.Components.Converters;
using AdvSearcher.VK.Parser.Components.Requests;
using AdvSearcher.VK.Parser.Components.VkParserChain;
using AdvSearcher.VK.Parser.Components.VkParserChain.Nodes;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.VK.Parser.DependencyInjection;

public class VkParserServices : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public VkParserServices(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<VkDateConverter>()
            .AddScoped<VkParserPipeLine>()
            .AddScoped<IParser, VkParser>(p =>
            {
                IVkParserNode node = p.GetRequiredService<IVkParserNode>();
                VkParser parser = new VkParser(node);
                parser.SetMessageListener(_listener);
                return parser;
            })
            .AddScoped<IVKOptionsProvider, VkOptionsProvider>()
            .AddScoped<IVkParserRequestFactory, VkParserRequestFactory>()
            .AddScoped<IVkParserNode>(p =>
            {
                VkParserPipeLine pipeLine = p.GetRequiredService<VkParserPipeLine>();
                IHttpService httpService = p.GetRequiredService<IHttpService>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();
                IVkParserRequestFactory requestFactory =
                    p.GetRequiredService<IVkParserRequestFactory>();

                IVkParserNode responseNode = new CreateVkParserResponseNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    requestFactory,
                    _listener
                );

                IVkParserNode itemJsonNode = new CreateVkItemsJsonNode(
                    pipeLine,
                    httpClient,
                    httpService,
                    requestFactory,
                    _listener,
                    responseNode
                );

                IVkParserNode groupInfoNode = new CreateVkGroupInfoNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    _listener,
                    itemJsonNode
                );

                IVkParserNode parametersNode = new CreateRequestParametersNode(
                    pipeLine,
                    _listener,
                    groupInfoNode
                );
                return parametersNode;
            });
        _listener.Publish("Парсер ВК подргужен.");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
