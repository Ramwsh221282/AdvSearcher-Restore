using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository;

public interface IAdvertisementsRepository
{
    Task<Result<IEnumerable<Advertisement>>> AddAdvertisements(
        IEnumerable<Advertisement> advertisements
    );
    Task<Result<Advertisement>> AddAdvertisement(Advertisement advertisement);
    Task<Result<Advertisement>> RemoveAdvertisement(Advertisement advertisement);
    Task CleanAdvertisements();
    Task<IEnumerable<Advertisement>> GetPagedAdvertisements(int page, int pageSize);
}
