using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class GetGroupIdRequest : IVkPublishingRequest
{
    private readonly string _screenName;
    private readonly ResponsesContainer _container;
    private readonly VkPublishingTokens _tokens;
    private readonly VkPublishingHttpClient _client;
    private readonly PublishingLogger _logger;

    public GetGroupIdRequest(
        ServiceUrl url,
        ResponsesContainer container,
        VkPublishingTokens tokens,
        VkPublishingHttpClient client,
        PublishingLogger logger
    )
    {
        Uri uri = new Uri(url.Value.Value);
        _screenName = uri.AbsolutePath.Trim('/');
        _container = container;
        _tokens = tokens;
        _client = client;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Requesting group ID");
        if (_client.Client is null)
        {
            _logger.Log("Http client is unavailable. Stopping");
            return;
        }
        RestRequest request = CreateRequest();
        RestResponse response = await _client.Client.ExecuteAsync(request);
        _logger.Log("Request completed");
        string? content = response.Content;
        if (string.IsNullOrEmpty(content))
        {
            _logger.Log("Request response was null");
            return;
        }
        _logger.Log("Parsing response to json");
        JObject json = JObject.Parse(content);
        JToken? responseToken = json["response"];
        if (responseToken is null)
        {
            _logger.Log("Response token was null stopping");
            return;
        }
        JToken? objectIdToken = responseToken["object_id"];
        if (objectIdToken is null)
        {
            _logger.Log("Object id was null stopping");
            return;
        }
        string objectIdValue = objectIdToken.ToString();
        _logger.Log($"Got response value. Group id: {objectIdValue}");
        _logger.Log("Processing next request");
        GroupIdResponse result = new GroupIdResponse(objectIdValue);
        _container.AddResponse(result);
    }

    private RestRequest CreateRequest()
    {
        string url =
            $"https://api.vk.com/method/utils.resolveScreenName?screen_name={_screenName}&access_token={_tokens.OAuthAccessToken}&v=5.131";
        return new RestRequest(url);
    }
}
