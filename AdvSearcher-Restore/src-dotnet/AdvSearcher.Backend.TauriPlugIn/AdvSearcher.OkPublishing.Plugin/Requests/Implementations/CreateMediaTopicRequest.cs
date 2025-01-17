using AdvSearcher.Core.Tools;
using AdvSearcher.OkPublishing.Plugin.Responses;
using AdvSearcher.Publishing.SDK.Models;
using Newtonsoft.Json;

namespace AdvSearcher.OkPublishing.Plugin.Requests.Implementations;

internal sealed class CreateMediaTopicRequest : IOkRequest
{
    private readonly PublishingLogger _logger;
    private const string TextFile = "Text.txt";
    private readonly ResponseContainer _container;
    private readonly AdvertisementFileResponse _file;
    private readonly OkTokens _tokens;

    public CreateMediaTopicRequest(
        ResponseContainer container,
        AdvertisementFileResponse file,
        OkTokens tokens,
        PublishingLogger logger
    )
    {
        _container = container;
        _file = file;
        _tokens = tokens;
        _logger = logger;
    }

    public async Task Execute()
    {
        _logger.Log("Starting Create Media Topic request");
        _logger.Log("Requesting group id responses from container");
        Result<GroupIdResponse> groupId = _container.GetRequiredResponseOfType<GroupIdResponse>();
        if (groupId.IsFailure)
        {
            _logger.Log($"Got error: {groupId.Error.Description}");
            _logger.Log("Stopping process");
            return;
        }
        _logger.Log("Creating request parameters");
        List<CommitPhotoResponse> commitedPhotos =
            _container.GetRequiredResponsesOfType<CommitPhotoResponse>();

        string text = File.ReadAllText(Path.Combine(_file.Path, TextFile));

        using (HttpClient client = new HttpClient { BaseAddress = new Uri("https://api.ok.ru/") })
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "application_key", _tokens.PublicToken },
                    { "method", "mediatopic.post" },
                    { "gid", groupId.Value.Id },
                    { "type", "GROUP_THEME" },
                    {
                        "attachment",
                        JsonConvert.SerializeObject(
                            new
                            {
                                media = new List<object>
                                {
                                    new { type = "text", text },
                                    new
                                    {
                                        type = "photo",
                                        list = new List<object>(
                                            commitedPhotos.Select(id => new { id = id.Token })
                                        ),
                                    },
                                },
                            }
                        )
                    },
                    { "format", "json" },
                    { "access_token", _tokens.LongToken },
                }
            );
            _logger.Log("Sending request");
            HttpResponseMessage responseMessage = await client.PostAsync("/fb.do", content);
            _logger.Log("Request sended");
            string result = await responseMessage.Content.ReadAsStringAsync();
            _logger.Log(result);
        }
    }
}
