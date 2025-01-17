namespace AdvSearcher.Publishing.SDK.Models;

public sealed class PublishingAdvertisementsFileSystem
{
    private static readonly string RootPath = $@"{Environment.CurrentDirectory}\Data";

    public List<AdvertisementDirectoryResponse> GetAdvertisementDirectories()
    {
        AdvertisementDirectory directory = new AdvertisementDirectory(RootPath);
        directory.FetchInside();
        List<AdvertisementDirectory> directores = [];
        directores.AddRange(directory.GetDirectoriesWithAdvertisements());
        List<AdvertisementDirectoryResponse> _responses = [];
        foreach (var folder in directores)
        {
            _responses.Add(directory.ConvertToResponse());
        }
        return _responses;
    }
}
