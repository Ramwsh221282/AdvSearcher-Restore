using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK.Models;
using AdvSearcher.VkPublishing.Plugin.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class PostImagesRequest : IVkPublishingRequest
{
    private readonly PublishingLogger _logger;
    private readonly VkPublishingHttpClient _client;
    private readonly AdvertisementFileResponse _file;
    private readonly ResponsesContainer _container;

    public PostImagesRequest(
        VkPublishingHttpClient client,
        AdvertisementFileResponse file,
        ResponsesContainer container,
        PublishingLogger logger
    )
    {
        _logger = logger;
        _client = client;
        _file = file;
        _container = container;
    }

    public async Task Execute()
    {
        _logger.Log("Executing Post Images Request");
        Result<UploadUrlResponse> uploadUrl = _container.GetResponseOfType<UploadUrlResponse>();
        _logger.Log("Getting upload Url responses from container");
        if (uploadUrl.IsFailure)
        {
            _logger.Log("Upload url responses were not found. Stopping process");
            return;
        }
        if (_client.Client is null)
        {
            _logger.Log("Http client is unavailable. Stopping process.");
            return;
        }
        if (_file.Photos.Count == 0)
        {
            _logger.Log("File has no photos. Stopping request");
            return;
        }
        _logger.Log("Start doing requests for each photo in advertisement");
        Task<RestResponse>[] tasks = new Task<RestResponse>[_file.Photos.Count];
        for (int index = 0; index < _file.Photos.Count; index++)
        {
            RestRequest request = new RestRequest(uploadUrl.Value.UploadUrl, Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddFile("photo", _file.Photos[index]);
            tasks[index] = _client.Client.ExecuteAsync(request);
            _logger.Log($"Executing request {index}");
        }
        RestResponse[] responses = await Task.WhenAll(tasks);
        _logger.Log("Requests execution finished. Parsing results.");
        if (responses.Any(r => string.IsNullOrWhiteSpace(r.Content)))
        {
            _logger.Log("Some of the response was without content. Stopping process");
            return;
        }
        foreach (var result in responses)
        {
            string? content = result.Content;
            if (content is null)
            {
                _logger.Log("Response content is null. Stopping process");
                break;
            }
            JObject json = JObject.Parse(content);
            JToken? serverToken = json["server"];
            JToken? photoToken = json["photo"];
            JToken? hashToken = json["hash"];
            if (serverToken is null || photoToken is null || hashToken is null)
                break;
            string server = serverToken.ToString();
            string photo = photoToken.ToString();
            string hash = hashToken.ToString();
            _logger.Log($"Parsed request content. Server: {server} Photo: {photo} Hash: {hash}");
            _logger.Log("Processing next request");
            _container.AddResponse(new UploadServerResponse(server, photo, hash));
        }
    }
}
