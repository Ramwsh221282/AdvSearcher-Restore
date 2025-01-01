using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Publishers;

public readonly record struct PublisherId
{
    public int Value { get; }

    private PublisherId(int value) => Value = value;

    public PublisherId()
    {
        Value = 0;
    } // ef core constructor

    internal static Result<PublisherId> CreateEmpty() => new PublisherId(0);

    public static PublisherId Create(int value) => new PublisherId(value);
}
