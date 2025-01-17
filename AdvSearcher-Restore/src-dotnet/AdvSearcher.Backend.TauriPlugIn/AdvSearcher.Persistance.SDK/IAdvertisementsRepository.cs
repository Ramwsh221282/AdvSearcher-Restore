using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Persistance.SDK;

public interface IAdvertisementsRepository
{
    Task<Result<RepositoryOperationResult>> Add(Advertisement advertisement);
    Task<Result<RepositoryOperationResult>> Delete(Advertisement advertisement);
    Task<Result<RepositoryOperationResult>> Clear();
    Task<IEnumerable<Advertisement>> GetAll(AdvertisementsDbQuery query);
    Task<Result<bool>> IsDuplicateAdvertisement(Advertisement advertisement);
    Task<IEnumerable<ExistingAdvertisementTokens>> GetExistingAdvertisementTokens();
    Task<Result<Advertisement>> GetById(AdvertisementId id);
    Task<int> GetCount(GetAdvertisementsCountByServiceQuery query);
}
