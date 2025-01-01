using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public sealed record ServiceUrlValue
{
    public string Value { get; init; }

    private ServiceUrlValue()
    {
        Value = string.Empty;
    } // ef core constructor

    private ServiceUrlValue(string value) => Value = value;

    public static Result<ServiceUrlValue> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return ServiceUrlsErrors.UrlEmpty;
        return new ServiceUrlValue(value);
    }
}
