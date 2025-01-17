using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.Errors;

public static class ServiceUrlsErrors
{
    public static readonly Error UrlEmpty = new("Ссылка не указана");
    public static readonly Error ModeEmpty = new("Режим ссылки не указан");
    public static readonly Error InvalidMode = new("Режим объявления не допустим");
    public static readonly Error Duplicate = new("Дубликат ссылки");
    public static readonly Error NotFound = new("Ссылка не была найдена");
    public static readonly Error EmptyServiceUrlName = new("Название сервиса ссылки не указано");
}
