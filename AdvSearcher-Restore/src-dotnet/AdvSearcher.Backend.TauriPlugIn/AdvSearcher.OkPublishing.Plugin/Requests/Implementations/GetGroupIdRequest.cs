using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.OkPublishing.Plugin.Responses;
using AdvSearcher.OkPublishing.Plugin.Utils;
using AdvSearcher.Publishing.SDK.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AdvSearcher.OkPublishing.Plugin.Requests.Implementations;

internal sealed record GroupIdResponse(string Id) : OkResponse;

internal sealed class GetGroupIdRequest : IOkRequest
{
    private readonly PublishingLogger _logger;
    private readonly OkHttpService _service;
    private readonly OkTokens _tokens;
    private readonly ServiceUrl _url;
    private readonly ResponseContainer _container;

    public GetGroupIdRequest(
        OkHttpService service,
        OkTokens tokens,
        ServiceUrl url,
        ResponseContainer container,
        PublishingLogger logger
    )
    {
        _logger = logger;
        _service = service;
        _tokens = tokens;
        _url = url;
        _container = container;
    }

    public async Task Execute()
    {
        _logger.Log("Starting get group id request");
        OkRequest request = CreateRequest();
        Result<string> response = await _service.ExecuteAsync(request);
        if (response.IsFailure)
        {
            _logger.Log($"Request executed with error: {response.Error.Description}");
            return;
        }
        _logger.Log("Got response. Parsing response.");
        JObject json = JObject.Parse(response);
        JToken? token = json.SelectToken("objectId");
        if (token is null)
        {
            _logger.Log("Response token object id was not found in json. Stopping process");
            return;
        }
        string tokenValue = token.ToString();
        _container.AddResponse(new GroupIdResponse(tokenValue));
        _logger.Log($"Response parsed. Token: {tokenValue}");
    }

    private OkRequest CreateRequest()
    {
        RestRequest request = new RestRequest("/fb.do", Method.Post);
        request.AddParameter("url", _url.Value.Value);
        request.AddParameter("application_key", _tokens.PublicToken);
        request.AddParameter("access_token", _tokens.LongToken);
        request.AddParameter("method", "url.getInfo");
        request.AddParameter("format", "json");
        return new OkRequest(request);
    }
}
