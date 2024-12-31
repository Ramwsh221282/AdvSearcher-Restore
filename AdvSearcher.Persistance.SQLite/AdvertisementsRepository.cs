using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite;

internal sealed class AdvertisementsRepository : IAdvertisementsRepository
{
    private readonly AppDbContext _context;

    public AdvertisementsRepository(AppDbContext context) => _context = context;

    public async Task<Result<RepositoryOperationResult>> Add(Advertisement advertisement)
    {
        if (
            await _context.Advertisements.AnyAsync(ad =>
                ad.Id == advertisement.Id || ad.Content == advertisement.Content
            )
        )
            return new Error("Duplicate advertisement");
        await _context.Set<Advertisement>().AddAsync(advertisement);
        await _context.SaveChangesAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<Result<RepositoryOperationResult>> Delete(Advertisement advertisement)
    {
        if (!await _context.Advertisements.AnyAsync(a => a.Id == advertisement.Id))
            return new Error("Advertisement not found");
        await _context.Advertisements.Where(ad => ad.Id == advertisement.Id).ExecuteDeleteAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<Result<RepositoryOperationResult>> Clear()
    {
        await _context.Advertisements.Where(ad => ad.Id.Id > 0).ExecuteDeleteAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<IEnumerable<Advertisement>> GetAll() =>
        await _context
            .Advertisements.Include(ad => ad.Attachments)
            .Include(ad => ad.Publisher!)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

    public async Task<Result<bool>> IsDuplicateAdvertisement(Advertisement advertisement) =>
        await _context.Advertisements.AnyAsync(ad =>
            ad.Id == advertisement.Id || ad.Content == advertisement.Content
        );

    public async Task<Result<RepositoryOperationResult>> AddRange(
        IEnumerable<Advertisement> advertisements
    )
    {
        List<Advertisement> unique = [];
        foreach (Advertisement ad in advertisements)
        {
            if (!await IsDuplicateAdvertisement(ad))
                unique.Add(ad);
        }
        if (!unique.Any())
            return new Error("No unique advertisements");
        await _context.Set<Advertisement>().AddRangeAsync(unique);
        await _context.SaveChangesAsync();
        return RepositoryOperationResult.Success;
    }
}
