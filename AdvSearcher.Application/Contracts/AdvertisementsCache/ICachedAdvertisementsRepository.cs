using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Contracts.AdvertisementsCache;

public enum CachedAdvertisementOperationType
{
    Success,
}

public interface ICachedAdvertisementsRepository
{
    Task<Result<CachedAdvertisementOperationType>> Add(CachedAdvertisement advertisement);
    Task<Result<CachedAdvertisementOperationType>> Clear();
    Task<IEnumerable<CachedAdvertisement>> GetAll();
    Task<int> GetCacheCount();
}
