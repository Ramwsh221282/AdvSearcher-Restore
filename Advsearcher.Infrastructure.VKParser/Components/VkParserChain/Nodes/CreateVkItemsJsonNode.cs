using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateVkItemsJsonNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IVkParserRequestFactory _factory;
    private readonly ParserConsoleLogger _logger;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkItemsJsonNode(
        VkParserPipeLine pipeLine,
        IHttpClient httpClient,
        IHttpService service,
        IVkParserRequestFactory factory,
        ParserConsoleLogger logger,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = service;
        _factory = factory;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Creating Vk Items json from wall.");
        if (_pipeLine.GroupInfo == null)
        {
            _logger.Log("Group info was not initialized. Stopping process.");
            return;
        }
        IHttpRequest request = _factory.CreateWallPostRequest(
            _pipeLine.Options,
            _pipeLine.GroupInfo
        );
        VkItemsJsonFactory jsonFactory = new VkItemsJsonFactory(_httpService, request, _httpClient);
        Result<VkItemsJson> jsons = await jsonFactory.CreateVkItemsJson();
        if (jsons.IsFailure)
        {
            _logger.Log("Vk Wall post items were not initialized. Stopping process.");
            return;
        }
        _pipeLine.SetItemsJson(jsons.Value);
        _logger.Log("Vk Wall post items were initialized.");
        if (Next != null)
        {
            _logger.Log("Processing next step in chain.");
            await Next.ExecuteAsync();
        }
    }
}
