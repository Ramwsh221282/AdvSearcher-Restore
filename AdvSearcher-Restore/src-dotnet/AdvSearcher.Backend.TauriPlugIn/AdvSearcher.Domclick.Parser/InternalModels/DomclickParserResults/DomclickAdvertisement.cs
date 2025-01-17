using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Domclick.Parser.InternalModels.DomclickParserResults;

public sealed record DomclickAdvertisement : IParsedAdvertisement
{
    public string Id { get; init; }
    public string Url { get; init; }
    public string Content { get; init; }
    public DateOnly Date { get; init; }

    private DomclickAdvertisement(string id, string url, string content, DateOnly date)
    {
        Id = id;
        Url = url;
        Content = content;
        Date = date;
    }

    public static Result<IParsedAdvertisement> Create(DomclickFetchResult result)
    {
        if (string.IsNullOrWhiteSpace(result.Id))
            return ParserErrors.CantParseId;
        if (string.IsNullOrWhiteSpace(result.SourceUrl))
            return ParserErrors.CantParseUrl;
        if (result.PublishedDate == DateOnly.MinValue)
            return ParserErrors.CantConvertDate;
        if (string.IsNullOrWhiteSpace(result.Description))
            return ParserErrors.CantParseContent;
        return new DomclickAdvertisement(
            result.Id,
            result.SourceUrl,
            BuildDescription(result),
            result.PublishedDate
        );
    }

    private static string BuildDescription(DomclickFetchResult result)
    {
        StringBuilder stringBuilder = new StringBuilder(result.Description).AppendLine(
            $"Цена: {result.Price}"
        );
        return stringBuilder.ToString();
    }
}
