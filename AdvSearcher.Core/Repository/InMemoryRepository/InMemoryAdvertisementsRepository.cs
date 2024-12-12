using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository.InMemoryRepository;

public sealed class InMemoryAdvertisementsRepository : IAdvertisementsRepository
{
    private readonly List<Advertisement> _advertisements = [];

    public async Task<Result<IEnumerable<Advertisement>>> AddAdvertisements(
        IEnumerable<Advertisement> advertisements
    )
    {
        List<Advertisement> added = [];
        foreach (var advertisement in advertisements)
        {
            if (_advertisements.Any(ad => ad.Content == advertisement.Content))
                continue;
            _advertisements.Add(advertisement);
        }

        return await Task.FromResult(added);
    }

    public async Task<Result<Advertisement>> AddAdvertisement(Advertisement advertisement)
    {
        if (_advertisements.Any(ad => ad.Content == advertisement.Content))
            return AdvertisementErrors.Duplicate;
        _advertisements.Add(advertisement);
        return await Task.FromResult(advertisement);
    }

    public async Task<Result<Advertisement>> RemoveAdvertisement(Advertisement advertisement)
    {
        var isRemoved = _advertisements.Remove(advertisement);
        if (!isRemoved)
            return AdvertisementErrors.NotFound;
        return await Task.FromResult(advertisement);
    }

    public async Task CleanAdvertisements()
    {
        _advertisements.Clear();
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Advertisement>> GetPagedAdvertisements(int page, int pageSize) =>
        await Task.FromResult(_advertisements);
}
