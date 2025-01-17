using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Requests;
using AdvSearcher.VkPublishing.Plugin.Utils;

namespace AdvSearcher.VkPublishing.Plugin;

public sealed class VkPublishingService : IPublishingService
{
    private readonly VkPublishingTokensLoader _loader;
    private readonly VkPublishingHttpClient _client;
    private readonly PublishingLogger _logger;
    private VkPublishingPipeLine _pipeLine;

    public VkPublishingService(PublishingLogger logger)
    {
        _loader = new VkPublishingTokensLoader();
        _client = new VkPublishingHttpClient();
        _pipeLine = new VkPublishingPipeLine();
        _logger = logger;
    }

    public async Task Publish(
        IEnumerable<AdvertisementFileResponse> responses,
        ServiceUrl serviceUrl
    )
    {
        Result<VkPublishingTokens> tokens = _loader.Load();
        if (tokens.IsFailure)
        {
            _logger.Log("Vk tokens were not found. Aborting process");
            return;
        }
        if (serviceUrl.Service.Name != "VK")
        {
            _logger.Log(
                "Url argument of service doesn't belong service of VKontakte. Aborting process"
            );
            return;
        }
        _client.Initialize();
        ResponsesContainer container = new ResponsesContainer();
        _pipeLine.AddRequest(
            new GetGroupIdRequest(serviceUrl, container, tokens, _client, _logger)
        );
        await _pipeLine.Process();
        Result<GroupIdResponse> groupId = container.GetResponseOfType<GroupIdResponse>();
        if (groupId.IsFailure)
            return;
        foreach (var ad in responses)
        {
            container = new ResponsesContainer();
            _pipeLine = new VkPublishingPipeLine();
            _pipeLine = _pipeLine
                .AddRequest(new GetVkUploadUrlRequest(_client, container, tokens, groupId, _logger))
                .AddRequest(new PostImagesRequest(_client, ad, container, _logger))
                .AddRequest(new CommitImagesRequest(_client, container, tokens, groupId, _logger))
                .AddRequest(
                    new UploadAdvertisementRequest(container, _client, ad, tokens, groupId, _logger)
                );
            await _pipeLine.Process();
        }
        _client.Destroy();
    }
}
