using AdvSearcher.Application.Models;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Persistance.SDK;

public interface IAdvertisementsRepository
{
    Task<Result<RepositoryOperationResult>> Add(Advertisement advertisement);
    Task<Result<RepositoryOperationResult>> Delete(Advertisement advertisement);
    Task<Result<RepositoryOperationResult>> Clear();
    Task<IEnumerable<Advertisement>> GetAll();
    Task<Result<bool>> IsDuplicateAdvertisement(Advertisement advertisement);
    Task<Result<RepositoryOperationResult>> AddRange(IEnumerable<Advertisement> advertisements);
    Task<IEnumerable<ExistingAdvertisementTokens>> GetExistingAdvertisementTokens();
}
