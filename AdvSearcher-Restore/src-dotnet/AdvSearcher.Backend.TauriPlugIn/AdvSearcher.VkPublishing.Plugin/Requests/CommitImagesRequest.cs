using System.Text;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class CommitImagesRequest : IVkPublishingRequest
{
    private readonly PublishingLogger _logger;
    private readonly VkPublishingHttpClient _client;
    private readonly ResponsesContainer _container;
    private readonly VkPublishingTokens _tokens;
    private readonly GroupIdResponse _response;

    public CommitImagesRequest(
        VkPublishingHttpClient client,
        ResponsesContainer container,
        VkPublishingTokens tokens,
        GroupIdResponse response,
        PublishingLogger logger
    )
    {
        _client = client;
        _container = container;
        _tokens = tokens;
        _response = response;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Executing commit image request");
        if (_client.Client is null)
        {
            _logger.Log("Http client is not available. Stopping process");
            return;
        }
        _logger.Log("Requesting upload server responses from container");
        IEnumerable<UploadServerResponse> responses =
            _container.GetResponsesOfType<UploadServerResponse>();
        if (!responses.Any())
        {
            _logger.Log("Upload server responses were not found. Stopping process");
            return;
        }
        _logger.Log("Executing requests for each photo");
        foreach (var response in responses)
        {
            RestRequest request = CreateCommitPhotoRequest(response, _response);
            string result = string.Empty;
            while (string.IsNullOrWhiteSpace(result))
            {
                RestResponse httpResponse = await _client.Client.ExecuteAsync(request);
                if (string.IsNullOrWhiteSpace(httpResponse.Content))
                    continue;
                if (httpResponse.Content.Contains("error"))
                    continue;
                result = httpResponse.Content;
            }
            _logger.Log($"Got response: {result}");
            JObject json = JObject.Parse(result);
            JToken? responseToken = json["response"];
            if (responseToken is null)
            {
                _logger.Log("Response token was not found in json. Stopping process");
                break;
            }
            JToken? idToken = responseToken[0]?["id"];
            JToken? ownerIdToken = responseToken[0]?["owner_id"];
            if (idToken is null || ownerIdToken is null)
            {
                _logger.Log(
                    "Either id token or ownerIdtoken was not found in json. Stopping process"
                );
                break;
            }
            string id = idToken.ToString();
            string ownerId = ownerIdToken.ToString();
            CommitedPhotoResponse commitedPhoto = new CommitedPhotoResponse(id, ownerId);
            _logger.Log($"Parsed json response. Id: {id}, owner id: {ownerId}");
            _container.AddResponse(commitedPhoto);
        }
    }

    private RestRequest CreateCommitPhotoRequest(
        UploadServerResponse response,
        GroupIdResponse groupId
    )
    {
        StringBuilder requestBuilder = new StringBuilder();
        requestBuilder.Append("https://api.vk.com/method/photos.saveWallPhoto?access_token=");
        requestBuilder.Append(_tokens.OAuthAccessToken);
        requestBuilder.Append("&group_id=");
        requestBuilder.Append(groupId.Id);
        requestBuilder.Append("&photo=");
        requestBuilder.Append(response.Photo);
        requestBuilder.Append("&server=");
        requestBuilder.Append(response.Server);
        requestBuilder.Append("&hash=");
        requestBuilder.Append(response.Hash);
        requestBuilder.Append("&v=");
        requestBuilder.Append("5.131");
        return new RestRequest(requestBuilder.ToString());
    }
}
