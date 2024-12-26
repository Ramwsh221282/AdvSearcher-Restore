using AdvSearcher.Core.Tools;

namespace AdvSearcher.Parser.SDK.Contracts;

public static class ParserErrors
{
    public static readonly Error UrlIsNotForLoading =
        new("Ссылка не является ссылкой для загрузки");

    public static readonly Error HtmlEmpty =
        new("Не удалось получить разметку сраницы при парсинге");

    public static readonly Error CantConvertDate = new("Дату нельзя преобразовать");

    public static readonly Error CantParseUrl = new("Не удалось спарсить ссылку на объявление");

    public static readonly Error CantParseId = new("Не удалось спарсить ID объявления");

    public static readonly Error CantParsePublisher = new("Не удалось спарсить автора");

    public static readonly Error CantParseContent = new("Не удалось спарсить описание");

    public static readonly Error CantParseAttachmentUrl =
        new("Не удалось получить ссылку на вложение");

    public static readonly Error NoAdvertisementsParsed = new Error(
        "Не удалось спарсить объявления"
    );
}
