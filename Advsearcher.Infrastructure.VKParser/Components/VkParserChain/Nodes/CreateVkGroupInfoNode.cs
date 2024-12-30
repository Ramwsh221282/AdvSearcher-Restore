using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateVkGroupInfoNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    private readonly ParserConsoleLogger _logger;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkGroupInfoNode(
        VkParserPipeLine pipeLine,
        IHttpService httpService,
        IHttpClient httpClient,
        ParserConsoleLogger logger,
        IVkParserNode? node = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = httpService;
        _logger = logger;
        Next = node;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Creating VK group info.");
        if (_pipeLine.Parameters == null)
        {
            _logger.Log("VK Parameters were null. Stopping process.");
            return;
        }
        IHttpRequest request = new VkGroupOwnerIdRequest(_pipeLine.Options, _pipeLine.Parameters);
        VKGroupInfoResponseFactory factory = new VKGroupInfoResponseFactory(
            request,
            _httpClient,
            _httpService
        );
        Result<VkGroupInfo> info = await factory.CreateGroupInfo(_pipeLine.Parameters);
        if (info.IsFailure)
        {
            _logger.Log($"Vk Group info failed. Reason: {info.Error}");
            return;
        }
        _pipeLine.SetGroupInfo(info.Value);
        _logger.Log("Vk Group info has been set.");
        if (Next != null)
        {
            _logger.Log("Processing next step.");
            await Next.ExecuteAsync();
        }
    }
}
