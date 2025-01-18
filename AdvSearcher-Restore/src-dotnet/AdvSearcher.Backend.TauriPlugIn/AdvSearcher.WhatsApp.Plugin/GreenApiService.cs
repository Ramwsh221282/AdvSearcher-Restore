using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.WhatsApp.Plugin.Requests;
using AdvSearcher.WhatsApp.Plugin.Requests.Implementations;
using AdvSearcher.WhatsApp.Plugin.Utils;

namespace AdvSearcher.WhatsApp.Plugin;

public sealed class GreenApiService : IWhatsAppSender
{
    private readonly PublishingLogger _logger;
    private readonly GreenApiHttpClient _client = new GreenApiHttpClient();
    private Action<int>? _currentProgress;
    private Action<int>? _maxProgress;

    public GreenApiService(PublishingLogger logger)
    {
        _logger = logger;
    }

    public async Task Publish(
        IEnumerable<AdvertisementFileResponse> files,
        WhatsAppSendRequest request
    )
    {
        GreenApiTokensLoader loader = new GreenApiTokensLoader();
        Result<GreenApiTokens> tokens = loader.Load();
        if (tokens.IsFailure)
        {
            _logger.Log("Tokens for green api service were not found. Aborting process.");
            _client.Destroy();
            return;
        }
        GreenApiHttpService service = new GreenApiHttpService(_client);
        _maxProgress?.Invoke(files.Count());
        int currentProgress = 0;
        foreach (var file in files)
        {
            IRequest sendTextrequest = new SendTextRequest(service, tokens, file, request, _logger);
            await sendTextrequest.Process();
            IRequest sendPhotosRequest = new SendPhotosRequest(
                tokens,
                file,
                service,
                request,
                _logger
            );
            await sendPhotosRequest.Process();
            currentProgress = currentProgress + 1;
            _currentProgress?.Invoke(currentProgress);
        }
        _client.Destroy();
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _currentProgress = actionPublisher;

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _maxProgress = actionPublisher;
}
