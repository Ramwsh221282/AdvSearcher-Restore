namespace AdvSearcher.Core.Entities.Publishers.ValueObjects;

public readonly record struct PublisherId
{
    public int Value { get; init; }

    public PublisherId() => Value = 0; // EF Core constructor

    private PublisherId(int value) => Value = value;

    public static PublisherId Create(int value) => new PublisherId { Value = value };
}
