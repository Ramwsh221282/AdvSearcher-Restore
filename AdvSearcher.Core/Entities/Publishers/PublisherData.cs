using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Publishers;

public record PublisherData
{
    public string Value { get; }

    private PublisherData()
    {
        Value = string.Empty;
    } // ef core constructor

    private PublisherData(string value) => Value = value;

    public static Result<PublisherData> Create(string? value) =>
        string.IsNullOrWhiteSpace(value) ? PublisherErrors.InfoEmpty : new PublisherData(value);
}
