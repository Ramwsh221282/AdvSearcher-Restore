using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository.InMemoryRepository;

public sealed class InMemoryServiceUrlRepository : IServiceUrlRepository
{
    private readonly List<ServiceUrl> _serviceUrls = [];

    public int Count => _serviceUrls.Count;

    public async Task<Result<ServiceUrl>> AddServiceUrlAsync(ServiceUrl url)
    {
        if (_serviceUrls.Any(s => s.Url == url.Url && s.Mode == url.Mode))
            return ServiceUrlsErrors.Duplicate;
        _serviceUrls.Add(url);
        return await Task.FromResult(url);
    }

    public async Task<Result<ServiceUrl>> DropServiceUrlAsync(ServiceUrl url)
    {
        var isRemoved = _serviceUrls.Remove(url);
        if (!isRemoved)
            return ServiceUrlsErrors.NotFound;
        return await Task.FromResult(url);
    }

    public async Task<IEnumerable<ServiceUrl>> GetServiceUrlsByMode(ServiceUrlMode mode) =>
        await Task.FromResult(_serviceUrls.Where(s => s.Mode == mode));
}
