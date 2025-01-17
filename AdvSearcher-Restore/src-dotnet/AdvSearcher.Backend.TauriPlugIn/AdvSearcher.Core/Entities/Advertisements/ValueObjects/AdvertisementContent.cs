using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public sealed record AdvertisementContent
{
    public string Content { get; init; } = string.Empty;

    private AdvertisementContent() { } // EF Core Constructor

    private AdvertisementContent(string content) => Content = content;

    public static Result<AdvertisementContent> Create(string? content) =>
        content switch
        {
            null => AdvertisementErrors.ContentEmpty,
            not null when string.IsNullOrWhiteSpace(content) => AdvertisementErrors.ContentEmpty,
            _ => new AdvertisementContent(content),
        };
}
