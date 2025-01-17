using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.WhatsApp.Plugin.Utils;
using RestSharp;

namespace AdvSearcher.WhatsApp.Plugin.Requests.Implementations;

internal sealed class SendTextRequest : IRequest
{
    private readonly PublishingLogger _logger;
    private readonly GreenApiHttpService _service;
    private readonly GreenApiTokens _tokens;
    private readonly AdvertisementFileResponse _file;
    private readonly WhatsAppSendRequest _request;

    public SendTextRequest(
        GreenApiHttpService service,
        GreenApiTokens tokens,
        AdvertisementFileResponse file,
        WhatsAppSendRequest request,
        PublishingLogger logger
    )
    {
        _service = service;
        _tokens = tokens;
        _file = file;
        _request = request;
        _logger = logger;
    }

    public async Task Process()
    {
        _logger.Log("Recreating request payload to corrected (phone number fix)");
        Result<WhatsAppSendRequest> corrected = WhatsAppSendRequestFactory.CreateCorrect(_request);
        if (corrected.IsFailure)
        {
            _logger.Log("Can't create corrected payload. Stopping process");
            return;
        }
        GreenApiHttpRequest request = CreateRequest(corrected);
        _logger.Log("Executing send text message request");
        Result<string> response = await _service.ExecuteAsync(request);
        if (response.IsFailure)
        {
            _logger.Log(
                $"Send text message request is failed. Error: {response.Error.Description}. Aborting process"
            );
            return;
        }
        _logger.Log($"Send text message request finished. Response: {response.Value}");
    }

    private GreenApiHttpRequest CreateRequest(WhatsAppSendRequest phone)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("https://api.green-api.com");
        stringBuilder.Append("/waInstance");
        stringBuilder.Append(_tokens.InstanceId);
        stringBuilder.Append("/sendMessage/");
        stringBuilder.Append(_tokens.Token);
        RestRequest request = new RestRequest(stringBuilder.ToString(), Method.Post);
        request.AddHeader("Content-Type", "application/json");
        string text = File.ReadAllText(
            Path.Combine(_file.Path, AdvertisementFileResponse.TextFilePath)
        );
        var data = new { chatId = phone.PhoneNumber, message = text };
        request.AddJsonBody(data);
        return new GreenApiHttpRequest(request);
    }
}
