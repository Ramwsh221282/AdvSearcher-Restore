using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using Microsoft.EntityFrameworkCore;

namespace AdvSearcher.Persistance.SQLite;

internal sealed class PublishersRepository : IPublishersRepository
{
    private readonly AppDbContext _context = new AppDbContext();

    public async Task<Result<RepositoryOperationResult>> Remove(Publisher publisher)
    {
        if (!await _context.Publishers.AnyAsync(p => p.Id == publisher.Id))
            return new Error("Publisher was not found");
        await _context.Publishers.Where(p => p.Id == publisher.Id).ExecuteDeleteAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<Result<RepositoryOperationResult>> Add(Publisher publisher)
    {
        if (await _context.Publishers.AnyAsync(p => p.Data == publisher.Data))
            return new Error("Publisher already exists");
        await _context.Set<Publisher>().AddAsync(publisher);
        await _context.SaveChangesAsync();
        return RepositoryOperationResult.Success;
    }

    public async Task<IEnumerable<Publisher>> GetAll() =>
        await _context.Publishers.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Publisher>> GetOnlyIgnored() =>
        await _context.Publishers.Where(pub => pub.IsIgnored).AsNoTracking().ToListAsync();

    public async Task<Result<RepositoryOperationResult>> Update(Publisher publisher)
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
        _context.Publishers.Attach(publisher);
        _context.Entry(publisher).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RepositoryOperationResult.Success;
    }
}
