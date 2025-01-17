using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.Errors;

public static class AdvertisementErrors
{
    public static readonly Error EmptyId = new("Пустой идентификатор объявления");
    public static readonly Error InvalidId = new("Идентификатор объявлений некорректный");
    public static readonly Error EmptyUrl = new("Ссылка на источник объявления была пустой");
    public static readonly Error TypeEmpty = new("Тип объявления неопределёен");
    public static readonly Error ServiceNameEmpty = new("Название сервиса объявление было пустым");
    public static readonly Error AttachmentUrlEmpty = new("Ссылка на вложения пустая");
    public static readonly Error Duplicate = new("Дубликат объявления (по тексту)");
    public static readonly Error NotFound = new("Объявление не найдено");
    public static readonly Error ContentEmpty = new("Описание объявления было пустым");
}
