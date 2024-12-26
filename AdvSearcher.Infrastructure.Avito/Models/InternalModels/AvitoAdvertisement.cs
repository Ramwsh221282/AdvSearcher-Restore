using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Avito.Models.InternalModels;

internal sealed record AvitoAdvertisement : IParsedAdvertisement
{
    public string Id { get; init; }
    public string Url { get; init; }
    public string Content { get; init; }
    public DateOnly Date { get; init; }

    private AvitoAdvertisement(string id, string url, string content, DateOnly date)
    {
        Id = id;
        Url = url;
        Content = content;
        Date = date;
    }

    public static Result<IParsedAdvertisement> Create(
        AvitoCatalogueItem item,
        AvitoDateConverter converter
    )
    {
        if (string.IsNullOrWhiteSpace(item.Id))
            return ParserErrors.CantParseId;
        if (string.IsNullOrWhiteSpace(item.UrlInfo))
            return ParserErrors.CantParseUrl;
        if (string.IsNullOrWhiteSpace(item.Description))
            return ParserErrors.CantParseContent;
        Result<DateOnly> date = converter.Convert(item.DateInfo);
        if (date.IsFailure)
            return ParserErrors.CantConvertDate;
        return new AvitoAdvertisement(item.Id, item.UrlInfo, item.Description, date);
    }
}
