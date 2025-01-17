using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct CreationDate
{
    public DateOnly Date { get; init; }

    public CreationDate() => Date = default; // EF Core Constructor;

    private CreationDate(DateOnly date) => Date = date;

    public static Result<CreationDate> Create(DateOnly date) => new CreationDate(date);
}
