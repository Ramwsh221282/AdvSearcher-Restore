using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

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
        Result<string> id,
        Result<string> url,
        Result<string> content,
        Result<DateOnly> date
    )
    {
        if (id.IsFailure)
            return id.Error;
        if (url.IsFailure)
            return url.Error;
        if (content.IsFailure)
            return content.Error;
        if (date.IsFailure)
            return date.Error;
        return new OkParsedAdvertisement(id, url, content, date);
    }
}
