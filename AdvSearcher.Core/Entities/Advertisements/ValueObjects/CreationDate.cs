using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct CreationDate
{
    public DateOnly Date { get; init; }

    public CreationDate()
    {
        Date = default;
    } // ef core constructor

    private CreationDate(DateOnly date) => Date = date;

    public static Result<CreationDate> Create(DateOnly date)
    {
        if (date == default)
            return new Error("Incorrent advertisement date");
        return new CreationDate(date);
    }
}
