using AdvSearcher.Core.Tools;
using AdvSearcher.OkPublishing.Plugin.Responses;
using AdvSearcher.OkPublishing.Plugin.Utils;
using AdvSearcher.Publishing.SDK.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.OkPublishing.Plugin.Requests.Implementations;

internal sealed record OkUploadPhotoResponse(string Photo_Id, string UploadUrl) : OkResponse;

internal sealed class GetUploadUrlRequest : IOkRequest
{
    private readonly PublishingLogger _logger;
    private readonly OkHttpService _service;
    private readonly OkTokens _tokens;
    private readonly ResponseContainer _container;
    private readonly AdvertisementFileResponse _file;

    public GetUploadUrlRequest(
        OkHttpService serivce,
        OkTokens tokens,
        ResponseContainer container,
        AdvertisementFileResponse file,
        PublishingLogger logger
    )
    {
        _service = serivce;
        _tokens = tokens;
        _container = container;
        _file = file;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Starting Get Upload Url request");
        Result<GroupIdResponse> response = _container.GetRequiredResponseOfType<GroupIdResponse>();
        if (response.IsFailure)
        {
            _logger.Log($"Group ID response is failed. Error: {response.Error.Description}");
            return;
        }
        List<Task<Result<string>>> uploadRequests = [];
        _logger.Log("Executing request for each advertisement photo");
        foreach (var photo in _file.Photos)
        {
            OkRequest request = CreateRequest(response.Value.Id);
            uploadRequests.Add(_service.ExecuteAsync(request));
        }
        Result<string>[] results = await Task.WhenAll(uploadRequests);
        _logger.Log("Requests executed. Parsing responses");
        foreach (var result in results)
        {
            if (result.IsFailure)
            {
                _logger.Log($"Response error: {result.Error.Description}");
                continue;
            }
            JObject json = JObject.Parse(result);
            JToken? photoIdToken = json["photo_ids"];
            if (photoIdToken is null)
            {
                _logger.Log("Photo id was not found. Stopping process");
                continue;
            }
            JArray tokensArray = (JArray)photoIdToken;
            JToken photoIdTokenFirst = tokensArray[0];
            string photoId = photoIdTokenFirst.ToString();
            JToken? uploadUrlToken = json["upload_url"];
            if (uploadUrlToken is null)
            {
                _logger.Log("Upload url was not found. Stopping process");
                continue;
            }
            string uploadUrl = uploadUrlToken.ToString();
            _container.AddResponse(new OkUploadPhotoResponse(photoId, uploadUrl));
            _logger.Log($"Response added. Response value: {uploadUrl}");
        }
    }

    private OkRequest CreateRequest(string groupId)
    {
        RestRequest request = new RestRequest("/fb.do", Method.Post);
        request.AddParameter("gid", groupId);
        request.AddParameter("application_key", _tokens.PublicToken);
        request.AddParameter("access_token", _tokens.LongToken);
        request.AddParameter("method", "photosV2.getUploadUrl");
        request.AddParameter("format", "json");
        return new OkRequest(request);
    }
}
