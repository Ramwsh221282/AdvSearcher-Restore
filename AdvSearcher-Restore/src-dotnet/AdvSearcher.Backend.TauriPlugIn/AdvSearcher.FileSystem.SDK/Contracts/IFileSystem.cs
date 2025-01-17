using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.FileSystem.SDK.Contracts;

public record AdvertisementFolder(string Folder);

public interface IFileSystem : IListenable
{
    void MoveToSubfolder(AdvertisementFolder folder);
    void MoveToParent();
    IReadOnlyCollection<string> GetFolderNames();
    AdvertisementFileSystemResult SaveAdvertisementAsFile(
        Advertisement advertisement,
        AdvertisementFolder folder
    );
    AdvertisementFileSystemResult CreateDirectory(AdvertisementFolder folder);
}
