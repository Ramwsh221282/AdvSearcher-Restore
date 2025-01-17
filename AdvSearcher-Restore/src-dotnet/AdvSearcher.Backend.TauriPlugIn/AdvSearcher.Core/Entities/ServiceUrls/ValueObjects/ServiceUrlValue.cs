using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public sealed record ServiceUrlValue
{
    public string Value { get; init; } = string.Empty;

    private ServiceUrlValue() { } // EF Core constructor

    private ServiceUrlValue(string value) => Value = value;

    public static Result<ServiceUrlValue> Create(string? value) =>
        value switch
        {
            null => ServiceUrlsErrors.UrlEmpty,
            not null when string.IsNullOrWhiteSpace(value) => ServiceUrlsErrors.UrlEmpty,
            _ => new ServiceUrlValue(value),
        };
}
