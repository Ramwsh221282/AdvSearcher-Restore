using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.OkPublishing.Plugin.Requests;
using AdvSearcher.OkPublishing.Plugin.Requests.Implementations;
using AdvSearcher.OkPublishing.Plugin.Responses;
using AdvSearcher.OkPublishing.Plugin.Utils;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.OkPublishing.Plugin;

public sealed class OkPublishingService : IPublishingService
{
    private readonly PublishingLogger _logger;

    public OkPublishingService(PublishingLogger logger)
    {
        _logger = logger;
    }

    public async Task Publish(
        IEnumerable<AdvertisementFileResponse> selectedFiles,
        ServiceUrl serviceUrl
    )
    {
        if (serviceUrl.Mode == ServiceUrlMode.Loadable)
            return;
        if (serviceUrl.Service.Name != "OK")
            return;
        OkTokensLoader loader = new OkTokensLoader();
        Result<OkTokens> tokens = loader.Load();
        if (tokens.IsFailure)
            return;
        OkHttpClient client = new OkHttpClient();
        OkHttpService service = new OkHttpService(client);
        ResponseContainer container = new ResponseContainer();
        foreach (var file in selectedFiles)
        {
            OkRequestsPipeLine pipeLine = new OkRequestsPipeLine();
            pipeLine = pipeLine
                .AddRequest(new GetGroupIdRequest(service, tokens, serviceUrl, container, _logger))
                .AddRequest(new GetUploadUrlRequest(service, tokens, container, file, _logger))
                .AddRequest(new CommitUploadPhotoRequest(file, container, _logger))
                .AddRequest(new CreateMediaTopicRequest(container, file, tokens, _logger));
            await pipeLine.ProcessAll();
            container.CleanContainer();
        }
        client.Destroy();
    }
}
