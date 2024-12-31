using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public record ServiceUrlService
{
    public string Name { get; init; }

    private ServiceUrlService(string name) => Name = name;

    public static Result<ServiceUrlService> Create(string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? new Error("Empty service url name")
            : new ServiceUrlService(name);
}
