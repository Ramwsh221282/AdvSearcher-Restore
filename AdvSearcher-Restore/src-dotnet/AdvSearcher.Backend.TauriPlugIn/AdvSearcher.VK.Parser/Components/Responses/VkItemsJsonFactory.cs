using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.VK.Parser.Components.Responses;

public sealed class VkItemsJsonFactory
{
    private readonly IHttpRequest _request;
    private readonly IHttpClient _client;
    private readonly IHttpService _service;

    public VkItemsJsonFactory(IHttpService service, IHttpRequest request, IHttpClient client)
    {
        _request = request;
        _client = client;
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<Result<VkItemsJson>> CreateVkItemsJson()
    {
        await _service.Execute(_client, _request);
        string response = _service.Result;
        if (string.IsNullOrEmpty(response))
            return new Error("Не удалось получить посты ВК");
        return new VkItemsJson(response);
    }
}
