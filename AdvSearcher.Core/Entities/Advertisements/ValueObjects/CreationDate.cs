using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public sealed record CreationDate
{
    public DateOnly Date { get; init; }

    private CreationDate(DateOnly date) => Date = date;

    public static Result<CreationDate> Create(DateOnly date)
    {
        if (date == default)
            return new Error("Incorrent advertisement date");
        return new CreationDate(date);
    }
}
