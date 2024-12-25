using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

internal sealed class VkAdvertisement : IParsedAdvertisement
{
    public string Id { get; init; }
    public string Content { get; init; }
    public DateOnly Date { get; init; }
    public string Url { get; init; }

    private VkAdvertisement(string id, string text, DateOnly date, string url)
    {
        Id = id;
        Content = text;
        Date = date;
        Url = url;
    }

    public static Result<IParsedAdvertisement> Create(
        JToken json,
        VkGroupInfo info,
        IAdvertisementDateConverter<VkParser> converter
    )
    {
        var idToken = json["id"];
        if (idToken == null)
            return new Error("ИД объявления ВК было пустым");

        var textToken = json["text"];
        if (textToken == null)
            return new Error("Текст объявления ВК был пустым");

        var dateToken = json["date"];
        if (dateToken == null)
            return new Error("Дата объявления ВК была пустая");

        var id = idToken.ToString();
        var text = textToken.ToString();

        var date = converter.Convert(dateToken.ToString());
        if (date.IsFailure)
            return new Error("Не удалось получить дату объявления");

        var sourceUrl = $@"{info.GroupUrl}?w=wall-{info.GroupId}_{id}";
        return new VkAdvertisement(id, text, date, sourceUrl);
    }
}
