using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateVkGroupInfoNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkGroupInfoNode(
        VkParserPipeLine pipeLine,
        IHttpService httpService,
        IHttpClient httpClient,
        IVkParserNode? node = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = httpService;
        Next = node;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.Parameters == null)
            return;
        IHttpRequest request = new VkGroupOwnerIdRequest(_pipeLine.Options, _pipeLine.Parameters);
        VKGroupInfoResponseFactory factory = new VKGroupInfoResponseFactory(
            request,
            _httpClient,
            _httpService
        );
        Result<VkGroupInfo> info = await factory.CreateGroupInfo(_pipeLine.Parameters);
        if (info.IsFailure)
            return;
        _pipeLine.SetGroupInfo(info.Value);
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
