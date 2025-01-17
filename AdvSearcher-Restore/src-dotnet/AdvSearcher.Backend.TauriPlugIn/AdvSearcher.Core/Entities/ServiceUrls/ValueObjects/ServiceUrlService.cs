using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public sealed record ServiceUrlService
{
    public string Name { get; init; } = string.Empty;

    private ServiceUrlService() { } // ef core constructor

    private ServiceUrlService(string name) => Name = name;

    public static Result<ServiceUrlService> Create(string? name) =>
        name switch
        {
            null => ServiceUrlsErrors.EmptyServiceUrlName,
            not null when string.IsNullOrWhiteSpace(name) => ServiceUrlsErrors.EmptyServiceUrlName,
            _ => new ServiceUrlService(name),
        };
}
