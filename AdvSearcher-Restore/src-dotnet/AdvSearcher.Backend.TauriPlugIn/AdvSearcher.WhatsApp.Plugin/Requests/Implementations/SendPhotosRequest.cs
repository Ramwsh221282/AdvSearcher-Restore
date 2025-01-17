using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.WhatsApp.Plugin.Utils;
using RestSharp;

namespace AdvSearcher.WhatsApp.Plugin.Requests.Implementations;

internal sealed class SendPhotosRequest : IRequest
{
    private readonly PublishingLogger _logger;
    private readonly GreenApiTokens _tokens;
    private readonly AdvertisementFileResponse _file;
    private readonly GreenApiHttpService _service;
    private readonly WhatsAppSendRequest _incorrect;

    public SendPhotosRequest(
        GreenApiTokens tokens,
        AdvertisementFileResponse file,
        GreenApiHttpService service,
        WhatsAppSendRequest incorrect,
        PublishingLogger logger
    )
    {
        _tokens = tokens;
        _file = file;
        _service = service;
        _incorrect = incorrect;
        _logger = logger;
    }

    public async Task Process()
    {
        _logger.Log("Recreating request payload to corrected (mobile phone fix)");
        Result<WhatsAppSendRequest> corrected = WhatsAppSendRequestFactory.CreateCorrect(
            _incorrect
        );
        if (corrected.IsFailure)
        {
            _logger.Log("Can't create correct version of payload. Stopping process");
            return;
        }
        _logger.Log("Executing request for each photo in advertisement");
        foreach (var photo in _file.Photos)
        {
            GreenApiHttpRequest request = CreateRequest(corrected, photo);
            Result<string> response = await _service.ExecuteAsync(request);
            if (response.IsFailure)
                _logger.Log($"Response error: {response.Error.Description}");
            _logger.Log($"Response finished: {response.Value}");
        }
    }

    private GreenApiHttpRequest CreateRequest(WhatsAppSendRequest corrected, string photo)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("https://api.green-api.com");
        stringBuilder.Append("/waInstance");
        stringBuilder.Append(_tokens.InstanceId);
        stringBuilder.Append("/sendFileByUpload/");
        stringBuilder.Append(_tokens.Token);
        RestRequest request = new RestRequest(stringBuilder.ToString(), Method.Post);
        request.MultipartFormQuoteParameters = true;
        request.AddParameter("chatId", corrected.PhoneNumber);
        request.AddFile("file", photo);
        return new GreenApiHttpRequest(request);
    }
}
