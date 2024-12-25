using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.Responses;

internal sealed class VKGroupInfoResponseFactory
{
    private readonly IHttpRequest _request;
    private readonly IHttpClient _httpClient;
    private readonly IHttpService _service;

    public VKGroupInfoResponseFactory(
        IHttpRequest request,
        IHttpClient httpClient,
        IHttpService service
    )
    {
        _request = request;
        _httpClient = httpClient;
        _service = service;
    }

    public async Task<Result<VkGroupInfo>> CreateGroupInfo(VkRequestParameters parameters)
    {
        await _service.Execute(_httpClient, _request);
        string response = _service.Result;
        if (string.IsNullOrWhiteSpace(response))
            return new Error("Не удалось получить информацию о группе");
        return new VkGroupInfo(new VkGroupResponse(response), parameters.BaseUrl);
    }
}
