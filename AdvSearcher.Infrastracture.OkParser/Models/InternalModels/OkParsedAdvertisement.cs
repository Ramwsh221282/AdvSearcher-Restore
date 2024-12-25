using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastracture.OkParser.Models.InternalModels;

internal sealed record OkParsedAdvertisement : IParsedAdvertisement
{
    public string Id { get; init; }
    public string Url { get; init; }
    public string Content { get; init; }
    public DateOnly Date { get; init; }

    private OkParsedAdvertisement(string id, string url, string content, DateOnly date)
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
            return AdvertisementErrors.EmptyId;

        if (string.IsNullOrWhiteSpace(url))
            return AdvertisementErrors.EmptyUrl;

        if (string.IsNullOrWhiteSpace(content))
            return AdvertisementErrors.ContentEmpty;

        return new OkParsedAdvertisement(id, url, content, date);
    }
}
