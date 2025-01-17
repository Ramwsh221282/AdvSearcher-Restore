using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite;

public sealed class AdvertisementsRepository : IAdvertisementsRepository
{
    private readonly AppDbContext _context;

    public AdvertisementsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RepositoryOperationResult>> Add(Advertisement advertisement)
    {
        if (
            await _context.Advertisements.AnyAsync(ad =>
                ad.Id == advertisement.Id || ad.Content == advertisement.Content
            )
        )
            return new Error("Duplicate advertisement");
        await AttachPublisherIfDuplicated(advertisement);
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

    public async Task<IEnumerable<Advertisement>> GetAll(AdvertisementsDbQuery query)
    {
        IEnumerable<Advertisement> advertisements = await _context
            .Advertisements.ApplyQueryParams(query)
            .Include(ad => ad.Publisher)
            .Include(ad => ad.Attachments)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
        advertisements = advertisements.OrderByDescending(ad => ad.Date.Value);
        return advertisements;
    }

    public async Task<Result<bool>> IsDuplicateAdvertisement(Advertisement advertisement) =>
        await _context.Advertisements.AnyAsync(ad =>
            ad.Id == advertisement.Id || ad.Content == advertisement.Content
        );

    public async Task<IEnumerable<ExistingAdvertisementTokens>> GetExistingAdvertisementTokens()
    {
        var tokens = await _context
            .Advertisements.Select(ad => new { ad.ServiceName, ad.Id })
            .ToListAsync();
        return tokens.Select(t => new ExistingAdvertisementTokens(t.Id, t.ServiceName));
    }

    public async Task<Result<Advertisement>> GetById(AdvertisementId id)
    {
        Advertisement? advertisement = await _context
            .Advertisements.Include(ad => ad.Publisher)
            .Include(ad => ad.Attachments)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(ad => ad.Id == id);
        return advertisement == null ? new Error("Объявление не было найдено") : advertisement;
    }

    public async Task<int> GetCount(GetAdvertisementsCountByServiceQuery query)
    {
        int count = await _context.Advertisements.CountByServiceQuery(query).CountAsync();
        return count;
    }

    private async Task AttachPublisherIfDuplicated(Advertisement advertisement)
    {
        if (advertisement.Publisher == null)
            return;

        Publisher? publisher = await _context.Publishers.FirstOrDefaultAsync(pub =>
            pub.Data == advertisement.Publisher.Data
        );

        if (publisher == null)
            return;

        advertisement.Publisher = publisher;
        _context.Publishers.Attach(publisher);
    }
}
