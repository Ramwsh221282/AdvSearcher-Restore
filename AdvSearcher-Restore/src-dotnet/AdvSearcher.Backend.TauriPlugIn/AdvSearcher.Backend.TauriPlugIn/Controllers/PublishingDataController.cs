using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public class PublishingDataController
{
    public IEnumerable<AdvertisementDirectoryResponse> GetDirectories()
    {
        try
        {
            PublishingAdvertisementsFileSystem system = new PublishingAdvertisementsFileSystem();
            return system.GetAdvertisementDirectories();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            return [];
        }
    }
}
