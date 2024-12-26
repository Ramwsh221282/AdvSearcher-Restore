using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Models.VkParsedData;

internal sealed record VkParsedAdvertisement : IParsedAdvertisement
{
    public string Id { get; init; }
    public string Url { get; init; }
    public string Content { get; init; }
    public DateOnly Date { get; init; }

    internal VkParsedAdvertisement(string id, string url, string content, DateOnly date)
    {
        Id = id;
        Url = url;
        Content = content;
        Date = date;
    }
}
