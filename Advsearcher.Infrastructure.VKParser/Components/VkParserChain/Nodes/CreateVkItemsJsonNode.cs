using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateVkItemsJsonNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IVkParserRequestFactory _factory;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkItemsJsonNode(
        VkParserPipeLine pipeLine,
        IHttpClient httpClient,
        IHttpService service,
        IVkParserRequestFactory factory,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = service;
        _factory = factory;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.GroupInfo == null)
            return;
        IHttpRequest request = _factory.CreateWallPostRequest(
            _pipeLine.Options,
            _pipeLine.GroupInfo
        );
        VkItemsJsonFactory jsonFactory = new VkItemsJsonFactory(_httpService, request, _httpClient);
        Result<VkItemsJson> jsons = await jsonFactory.CreateVkItemsJson();
        if (jsons.IsFailure)
            return;
        _pipeLine.SetItemsJson(jsons.Value);
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
