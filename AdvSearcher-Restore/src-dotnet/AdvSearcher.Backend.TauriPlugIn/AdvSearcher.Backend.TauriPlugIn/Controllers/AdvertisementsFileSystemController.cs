using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.FileSystem.SDK.Contracts;
using AdvSearcher.Persistance.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public record SaveAdvertisementRequest(ulong Id, string FolderName);

public sealed class AdvertisementsFileSystemController(
    IFileSystem system,
    IAdvertisementsRepository repository,
    IEventPublisher publisher
)
{
    private const string Listener = "file-system-listener";
    private readonly IFileSystem _system = system;
    private readonly IAdvertisementsRepository _repository = repository;
    private readonly IEventPublisher _publisher = publisher;

    public IReadOnlyCollection<string> GetSubfolders() => _system.GetFolderNames();

    public IReadOnlyCollection<string> MoveToFolder(AdvertisementFolder folder)
    {
        try
        {
            _system.MoveToSubfolder(folder);
            return _system.GetFolderNames();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            return [];
        }
    }

    public IReadOnlyCollection<string> MoveToParent()
    {
        try
        {
            _system.MoveToParent();
            return _system.GetFolderNames();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            return [];
        }
    }

    public void SaveAdvertisement(SaveAdvertisementRequest request)
    {
        try
        {
            Result<AdvertisementId> id = AdvertisementId.Create(request.Id);
            if (id.IsFailure)
            {
                _publisher.Publish(Listener, id.Error.Description);
                return;
            }
            Result<Advertisement> advertisement = _repository.GetById(id).Result;
            if (advertisement.IsFailure)
            {
                _publisher.Publish(Listener, advertisement.Error.Description);
                return;
            }
            AdvertisementFileSystemResult result = _system.SaveAdvertisementAsFile(
                advertisement,
                new AdvertisementFolder(request.FolderName)
            );
            if (!result.IsSuccess)
                _publisher.Publish(Listener, result.ErrorMessage);
            else
                _publisher.Publish(Listener, "Объявление сохранено");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    public void CreateDirectory(AdvertisementFolder folder)
    {
        try
        {
            AdvertisementFileSystemResult result = _system.CreateDirectory(folder);
            if (!result.IsSuccess)
            {
                _publisher.Publish(Listener, result.ErrorMessage);
                return;
            }
            _publisher.Publish(Listener, "Папка создана");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}
