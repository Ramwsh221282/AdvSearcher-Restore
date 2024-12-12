using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public record AdvertisementContent
{
    public string Content { get; init; }

    private AdvertisementContent(string content) => Content = content;

    public static Result<AdvertisementContent> Create(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return new Error("Описание объявления было пустым");
        return new AdvertisementContent(content);
    }
}
