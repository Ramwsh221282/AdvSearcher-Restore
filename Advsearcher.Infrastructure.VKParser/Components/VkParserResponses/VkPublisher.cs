using System.Text.RegularExpressions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

internal sealed class VkPublisher : IParsedPublisher
{
    public string Info { get; set; } = string.Empty;

    public static Result<IParsedPublisher> Create(string postOwnerResponse)
    {
        string info = ExtractNames(postOwnerResponse);
        return new VkPublisher() { Info = info };
    }

    private static string ExtractNames(string data) =>
        $"{ExtractFirstName(data)} {ExtractLastName(data)}";

    private static string ExtractFirstName(string data)
    {
        var match = new Regex("\"first_name\"\\s*:\\s*\"(.*?)\"").Match(data);
        return match.Groups[1].Value;
    }

    private static string ExtractLastName(string data)
    {
        var match = new Regex("\"last_name\"\\s*:\\s*\"(.*?)\"").Match(data);
        return match.Groups[1].Value;
    }
}
