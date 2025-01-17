using System.Text.RegularExpressions;
using AdvSearcher.Core.Tools;
using AdvSearcher.OkPublishing.Plugin.Responses;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.OkPublishing.Plugin.Requests.Implementations;

internal sealed record CommitPhotoResponse(string Photo, string Token) : OkResponse;

internal sealed class CommitUploadPhotoRequest : IOkRequest
{
    private readonly PublishingLogger _logger;
    private const string PhotoRegexPattern = "{\"photos\":{\"(\\S+)\":{";
    private static readonly Regex PhotoRegex = new Regex(PhotoRegexPattern, RegexOptions.Compiled);
    private const string TokenRegexPattern = "\"token\":\"(\\S+)\"}}}";
    private static readonly Regex TokenRegex = new Regex(TokenRegexPattern, RegexOptions.Compiled);
    private readonly AdvertisementFileResponse _file;
    private readonly ResponseContainer _container;

    public CommitUploadPhotoRequest(
        AdvertisementFileResponse file,
        ResponseContainer container,
        PublishingLogger logger
    )
    {
        _file = file;
        _container = container;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Executing commit upload photo requests");
        List<OkUploadPhotoResponse> responses =
            _container.GetRequiredResponsesOfType<OkUploadPhotoResponse>();
        _logger.Log("Requesting Ok Upload Photo responses from response container");
        if (!responses.Any())
        {
            _logger.Log("Ok upload photos responses count was 0. Stopping process.");
            return;
        }
        using (HttpClient client = new HttpClient())
        {
            _logger.Log("Starting commit upload photo requests");
            List<Task<HttpResponseMessage>> tasks = [];
            for (int index = 0; index < responses.Count; index++)
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                byte[] imageBytes = File.ReadAllBytes(_file.Photos[index]);
                ByteArrayContent byteContent = new ByteArrayContent(imageBytes);
                string photoName = $"pic_{index}";
                content.Add(byteContent, photoName, _file.Photos[index]);
                tasks.Add(client.PostAsync(responses[index].UploadUrl, content));
                _logger.Log("Request sended");
            }
            HttpResponseMessage[] postResponses = await Task.WhenAll(tasks);
            _logger.Log("Requests finidhed");
            foreach (var response in postResponses)
            {
                _logger.Log("Parsing request response");
                string httpResponse = await response.Content.ReadAsStringAsync();
                _logger.Log(httpResponse);
                _logger.Log("Creating parsed response representation");
                Result<CommitPhotoResponse> result = ParseResponse(httpResponse);
                if (result.IsFailure)
                {
                    _logger.Log($"Commit photo response has error: {result.Error.Description}");
                    continue;
                }
                _container.AddResponse(result);
                _logger.Log("Response registered in container");
            }
        }
    }

    private Result<CommitPhotoResponse> ParseResponse(string httpResponse)
    {
        Match photoMatch = PhotoRegex.Match(httpResponse);
        Match tokenMatch = TokenRegex.Match(httpResponse);
        if (!photoMatch.Success || !tokenMatch.Success)
            return new Error("Can't parse response");
        string photo = photoMatch.Groups[1].Value;
        string token = tokenMatch.Groups[1].Value;
        token = token.Replace("\\u003d", "").Trim();
        return new CommitPhotoResponse(photo, token);
    }
}
