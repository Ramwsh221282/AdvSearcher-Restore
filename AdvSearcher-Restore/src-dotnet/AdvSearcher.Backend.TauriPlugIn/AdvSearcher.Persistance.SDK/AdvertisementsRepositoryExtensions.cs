using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Persistance.SDK;

public static class AdvertisementsRepositoryExtensions
{
    public static async Task<int> AddInRepository(
        this IAdvertisementsRepository repository,
        IEnumerable<Advertisement> advertisements
    )
    {
        int added = 0;
        foreach (var advertisement in advertisements)
        {
            Result<RepositoryOperationResult> result = await repository.Add(advertisement);
            if (result.IsSuccess)
                added++;
        }
        return added;
    }
}
