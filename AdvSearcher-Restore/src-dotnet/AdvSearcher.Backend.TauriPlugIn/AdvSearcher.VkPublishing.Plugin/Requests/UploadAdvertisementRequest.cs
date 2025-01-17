using System.Text;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Utils;
using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class UploadAdvertisementRequest : IVkPublishingRequest
{
    private readonly PublishingLogger _logger;
    private readonly ResponsesContainer _container;
    private readonly VkPublishingHttpClient _client;
    private readonly AdvertisementFileResponse _file;
    private readonly VkPublishingTokens _tokens;
    private readonly GroupIdResponse _groupId;

    public UploadAdvertisementRequest(
        ResponsesContainer container,
        VkPublishingHttpClient client,
        AdvertisementFileResponse file,
        VkPublishingTokens tokens,
        GroupIdResponse groupId,
        PublishingLogger logger
    )
    {
        _container = container;
        _client = client;
        _file = file;
        _tokens = tokens;
        _groupId = groupId;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Executing upload advertisement request");
        if (_client.Client is null)
        {
            _logger.Log("Http client is unavailable. Stopping process");
            return;
        }
        RestRequest request = CreateWallPostRequest(_file, _groupId);
        RestResponse response = await _client.Client.ExecuteAsync(request);
        _logger.Log($"Request finished: {response.Content}");
    }

    private RestRequest CreateWallPostRequest(
        AdvertisementFileResponse file,
        GroupIdResponse groupId
    )
    {
        string textFilePath = Path.Combine(file.Path, "Text.txt");
        string attachments =
            file.Photos.Count == 0 ? string.Empty : CreateAttachmentsParameterString();
        StringBuilder requestBuilder = new StringBuilder();
        requestBuilder.Append("https://api.vk.com/method/wall.post?owner_id=-");
        requestBuilder.Append(groupId.Id);
        requestBuilder.Append("&message=");
        requestBuilder.Append(File.ReadAllText(textFilePath));
        requestBuilder.Append("&attachments=");
        requestBuilder.Append(attachments);
        requestBuilder.Append("&access_token=");
        requestBuilder.Append(_tokens.OAuthAccessToken);
        requestBuilder.Append("&v=");
        requestBuilder.Append("5.131");
        return new RestRequest(requestBuilder.ToString());
    }

    private string CreateAttachmentsParameterString()
    {
        IEnumerable<CommitedPhotoResponse> responses =
            _container.GetResponsesOfType<CommitedPhotoResponse>();
        if (!responses.Any())
            return string.Empty;
        List<string> args = [];
        foreach (var response in responses)
        {
            StringBuilder argumentBuilder = new StringBuilder();
            argumentBuilder.Append("photo");
            argumentBuilder.Append(response.OwnerId);
            argumentBuilder.Append("_");
            argumentBuilder.Append(response.Id);
            args.Add(argumentBuilder.ToString());
        }
        return string.Join(",", args);
    }
}
