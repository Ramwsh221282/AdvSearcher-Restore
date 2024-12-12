using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Publishers;

public static class PublisherErrors
{
    public static readonly Error IdEmpty = new("ИД автора объявления было пустым");
    public static readonly Error InfoEmpty = new("Информация об авторе была пустой");
}
