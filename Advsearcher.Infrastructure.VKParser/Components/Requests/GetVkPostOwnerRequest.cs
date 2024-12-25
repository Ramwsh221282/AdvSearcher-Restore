using System.Text.RegularExpressions;
using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class GetVkPostOwnerRequest : IHttpRequest
{
    private readonly RestRequest _request;
    public RestRequest Request => _request;

    public GetVkPostOwnerRequest(VkOptions vkOptions, string id)
    {
        var url =
            $"https://api.vk.com/method/users.get?user_ids={id}&fields=bdate,city,country&access_token={vkOptions.OAuthAccessToken}&v={vkOptions.ApiVesrion}";
        _request = new RestRequest(url);
    }

    // public async Task<string?> ExecuteAsync()
    // {
    //     string data;
    //     do
    //     {
    //         var response = await _client.ExecuteAsync(_request);
    //         data = response.Content!;
    //     } while (data.Contains("error"));
    //
    //     return ExtractNames(data);
    // }
    //
    // private string ExtractNames(string data) => $"{ExtractFirstName(data)} {ExtractLastName(data)}";
    //
    // private string ExtractFirstName(string data)
    // {
    //     var match = new Regex("\"first_name\"\\s*:\\s*\"(.*?)\"").Match(data);
    //     return match.Groups[1].Value;
    // }
    //
    // private string ExtractLastName(string data)
    // {
    //     var match = new Regex("\"last_name\"\\s*:\\s*\"(.*?)\"").Match(data);
    //     return match.Groups[1].Value;
    // }
}
