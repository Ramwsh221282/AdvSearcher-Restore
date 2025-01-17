using AdvSearcher.Core.Entities.Publishers.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Publishers.ValueObjects;

public sealed record PublisherData
{
    public string Value { get; init; }

    private PublisherData() => Value = string.Empty; // EF Core constructor

    private PublisherData(string value) => Value = value;

    public static Result<PublisherData> Create(string? value) =>
        value switch
        {
            null => PublisherErrors.InfoEmpty,
            not null when string.IsNullOrWhiteSpace(value) => PublisherErrors.InfoEmpty,
            _ => new PublisherData(value),
        };
}
