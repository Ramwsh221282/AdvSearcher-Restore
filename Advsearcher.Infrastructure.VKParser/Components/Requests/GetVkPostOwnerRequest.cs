using System.Text.RegularExpressions;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class GetVkPostOwnerRequest : IVkParserRequest
{
    private readonly RestRequest _request;
    private readonly RestClient _client;

    public GetVkPostOwnerRequest(RestClient httpClient, VkOptions vkOptions, VkPublisher publisher)
    {
        var url =
            $"https://api.vk.com/method/users.get?user_ids={publisher.Id}&fields=bdate,city,country&access_token={vkOptions.OAuthAccessToken}&v={vkOptions.ApiVesrion}";
        _request = new RestRequest(url);
        _client = httpClient;
    }

    public async Task<string?> ExecuteAsync()
    {
        string data;
        do
        {
            var response = await _client.ExecuteAsync(_request);
            data = response.Content!;
        } while (data.Contains("error"));

        return ExtractNames(data);
    }

    private string ExtractNames(string data) => $"{ExtractFirstName(data)} {ExtractLastName(data)}";

    private string ExtractFirstName(string data)
    {
        var match = new Regex("\"first_name\"\\s*:\\s*\"(.*?)\"").Match(data);
        return match.Groups[1].Value;
    }

    private string ExtractLastName(string data)
    {
        var match = new Regex("\"last_name\"\\s*:\\s*\"(.*?)\"").Match(data);
        return match.Groups[1].Value;
    }
}
