using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite.Configurations;

internal sealed class ServiceUrlRepository : IServiceUrlRepository
{
    private readonly AppDbContext _context;

    public ServiceUrlRepository(AppDbContext context) => _context = context;

    public async Task<Result<RepositoryOperationResult>> Add(ServiceUrl serviceUrl)
    {
        if (
            await _context.ServiceUrls.AnyAsync(s =>
                s.Value == serviceUrl.Value && serviceUrl.Mode == s.Mode
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
            !await _context.ServiceUrls.AnyAsync(s =>
                s.Value == serviceUrl.Value && serviceUrl.Mode == s.Mode
            )
        )
            return new Error("Such doesn't already exist");
        await _context
            .ServiceUrls.Where(s => s.Mode == serviceUrl.Mode && s.Id == serviceUrl.Id)
            .ExecuteDeleteAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<IEnumerable<ServiceUrl>> Get(
        ServiceUrlMode mode,
        ServiceUrlService service
    ) =>
        await _context
            .ServiceUrls.Where(s => s.Mode == mode && s.Service == service)
            .AsNoTracking()
            .ToListAsync();
}
