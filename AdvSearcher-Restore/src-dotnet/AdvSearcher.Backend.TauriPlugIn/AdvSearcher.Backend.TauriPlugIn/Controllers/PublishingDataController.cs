using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public class PublishingDataController
{
    public IEnumerable<AdvertisementDirectoryResponse> GetDirectories()
    {
        PublishingAdvertisementsFileSystem system = new PublishingAdvertisementsFileSystem();
        return system.GetAdvertisementDirectories();
    }
}
