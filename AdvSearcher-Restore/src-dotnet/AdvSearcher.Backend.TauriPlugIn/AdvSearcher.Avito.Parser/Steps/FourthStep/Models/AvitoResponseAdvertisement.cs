using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Avito.Parser.Steps.FourthStep.Converters;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Models;

public record AvitoResponseAdvertisement : IParsedAdvertisement
{
    public string Id { get; }
    public string Url { get; }
    public string Content { get; }
    public DateOnly Date { get; }

    private AvitoResponseAdvertisement(string id, string url, string content, DateOnly date)
    {
        Id = id;
        Url = url;
        Content = content;
        Date = date;
    }

    public static Result<IParsedAdvertisement> Create(
        AvitoAdvertisement advertisement,
        AvitoDateConverter converter
    )
    {
        string description = advertisement.Description.ToString();
        if (string.IsNullOrWhiteSpace(description))
            return new Error("Description is empty");
        Result<DateOnly> date = converter.Convert(advertisement.Date);
        if (date.IsFailure)
            return new Error("Date is invalid");
        return new AvitoResponseAdvertisement(
            advertisement.Id,
            advertisement.SourceUrl,
            description,
            date.Value
        );
    }
}
