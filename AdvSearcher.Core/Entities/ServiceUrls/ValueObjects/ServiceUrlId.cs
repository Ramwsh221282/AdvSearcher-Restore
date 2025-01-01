namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public readonly record struct ServiceUrlId
{
    public int Value { get; init; }

    internal ServiceUrlId(int value) => Value = value;

    internal static ServiceUrlId CreateEmpty() => new ServiceUrlId(0);

    public static ServiceUrlId Create(int value) => new ServiceUrlId(value);
}
