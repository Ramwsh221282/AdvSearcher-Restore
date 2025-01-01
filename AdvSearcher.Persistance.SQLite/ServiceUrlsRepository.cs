using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite;

internal sealed class ServiceUrlsRepository : IServiceUrlRepository
{
    private readonly AppDbContext _context = new AppDbContext();

    public async Task<Result<RepositoryOperationResult>> Add(ServiceUrl serviceUrl)
    {
        if (
            await _context.ServiceUrls.AnyAsync(url =>
                url.Value == serviceUrl.Value && url.Mode == serviceUrl.Mode
            )
        )
            return new Error("Such url already exists");
        await _context.Set<ServiceUrl>().AddAsync(serviceUrl);
        await _context.SaveChangesAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<Result<RepositoryOperationResult>> Remove(ServiceUrl serviceUrl)
    {
        if (
            !await _context.ServiceUrls.AnyAsync(url =>
                url.Value == serviceUrl.Value && url.Mode == serviceUrl.Mode
            )
        )
            return new Error("Such url doesn't exist");
        await _context
            .ServiceUrls.Where(url => url.Value == serviceUrl.Value && url.Mode == serviceUrl.Mode)
            .ExecuteDeleteAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<IEnumerable<ServiceUrl>> Get(
        ServiceUrlMode mode,
        ServiceUrlService service
    ) =>
        await _context
            .ServiceUrls.Where(url => url.Mode == mode && url.Service == service)
            .AsNoTracking()
            .ToListAsync();
}
