using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Cian.Parser.Models.InternalModels;

public sealed record CianAdvertisement : IParsedAdvertisement
{
    public string Id { get; }
    public string Url { get; }
    public string Content { get; }
    public DateOnly Date { get; }

    private CianAdvertisement(string id, string url, string content, DateOnly date)
    {
        Id = id;
        Url = url;
        Content = content;
        Date = date;
    }

    public static Result<IParsedAdvertisement> Create(
        string? id,
        string? url,
        string? content,
        DateOnly date
    )
    {
        if (string.IsNullOrWhiteSpace(id))
            return ParserErrors.CantParseId;

        if (string.IsNullOrWhiteSpace(url))
            return ParserErrors.CantParseUrl;

        if (string.IsNullOrWhiteSpace(content))
            return ParserErrors.CantParseContent;

        return new CianAdvertisement(id, url, content, date);
    }
}
