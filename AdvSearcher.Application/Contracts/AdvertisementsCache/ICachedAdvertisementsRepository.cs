using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Contracts.AdvertisementsCache;

public enum CachedAdvertisementOperationType
{
    Added,
    Deleted,
    AlreadyExists,
}

public interface ICachedAdvertisementsRepository
{
    Task<Result<CachedAdvertisementOperationType>> Add(CachedAdvertisement advertisement);
    Task<Result<CachedAdvertisementOperationType>> Clear();
    Task<Result<IEnumerable<CachedAdvertisement>>> GetAll();
    Task<Result<int>> GetCacheCount();
}
