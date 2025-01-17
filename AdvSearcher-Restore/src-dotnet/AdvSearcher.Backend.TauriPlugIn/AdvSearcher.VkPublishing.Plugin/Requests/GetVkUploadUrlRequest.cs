using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class GetVkUploadUrlRequest : IVkPublishingRequest
{
    private readonly VkPublishingHttpClient _client;
    private readonly ResponsesContainer _container;
    private readonly VkPublishingTokens _tokens;
    private readonly GroupIdResponse _groupId;
    private readonly PublishingLogger _logger;

    public GetVkUploadUrlRequest(
        VkPublishingHttpClient client,
        ResponsesContainer container,
        VkPublishingTokens tokens,
        GroupIdResponse groupId,
        PublishingLogger logger
    )
    {
        _client = client;
        _container = container;
        _tokens = tokens;
        _groupId = groupId;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Starting request to get VK upload links");
        if (_client.Client is null)
        {
            _logger.Log("Http client is not available. Stopping process");
            return;
        }
        _logger.Log("Executing request");
        RestRequest request = CreateRequest(_groupId);
        RestResponse response = await _client.Client.ExecuteAsync(request);
        string? content = response.Content;
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.Log("Request response was null. Stopping process.");
            return;
        }
        _logger.Log("Parsing request response");
        JObject json = JObject.Parse(content);
        JToken? responseToken = json["response"];
        if (responseToken is null)
        {
            _logger.Log("Response token was null. Stopping process");
            return;
        }
        JToken? uploadUrlToken = responseToken["upload_url"];
        if (uploadUrlToken is null)
        {
            _logger.Log("Upload url token was null. Stopping process");
            return;
        }
        string uploadUrlValue = uploadUrlToken.ToString();
        _logger.Log($"Received upload url: {uploadUrlValue}");
        UploadUrlResponse result = new UploadUrlResponse(uploadUrlValue);
        _container.AddResponse(result);
    }

    private RestRequest CreateRequest(GroupIdResponse response)
    {
        StringBuilder requestBuilder = new StringBuilder();
        requestBuilder.Append("https://api.vk.com/method/photos.getWallUploadServer?group_id=");
        requestBuilder.Append(response.Id);
        requestBuilder.Append("&access_token=");
        requestBuilder.Append(_tokens.OAuthAccessToken);
        requestBuilder.Append("&v=");
        requestBuilder.Append("5.131");
        return new RestRequest(requestBuilder.ToString());
    }
}
