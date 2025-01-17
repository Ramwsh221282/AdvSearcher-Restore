using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.FileSystem.Plugin.WritingOperations;
using AdvSearcher.FileSystem.Plugin.WritingOperations.WritingLogics;
using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin;

public sealed class AdvertisementFileSystem : IFileSystem
{
    private readonly DirectoryObserver _observer = new();
    private IMessageListener? _listener;

    public void MoveToSubfolder(AdvertisementFolder folder)
    {
        string folderName = folder.Folder.Split('\\')[^1];
        _listener?.Publish($"Переход в директорию: {folderName}");
        _observer.MoveToNextSubfolder(folder.Folder);
    }

    public void MoveToParent()
    {
        _listener?.Publish("Переход в родительскую директорию");
        _observer.MoveToPreviousFolder();
    }

    public IReadOnlyCollection<string> GetFolderNames()
    {
        _listener?.Publish("Подгрузка папок");
        return _observer.GetSubfolderNames();
    }

    public AdvertisementFileSystemResult SaveAdvertisementAsFile(
        Advertisement advertisement,
        AdvertisementFolder folder
    )
    {
        _listener?.Publish("Сохранение объявления");
        string folderPath = Path.Combine(_observer.CurrentDirectory.CurrentPath, folder.Folder);
        if (Directory.Exists(folderPath))
        {
            string message = "Ошибка: Такая папка уже существует";
            return new AdvertisementFileSystemResult(false, message);
        }
        Directory.CreateDirectory(folderPath);
        _listener?.Publish("Создание папки объявления");
        CompositeAdvertisementWritingLogic logics = new CompositeAdvertisementWritingLogic();
        logics.AddLogic(new SaveAdvertisementText(_observer.CurrentDirectory, folder.Folder));
        logics.AddLogic(new SaveAdvertisementPhotos(_observer.CurrentDirectory, folder.Folder));
        IAdvertisementWritingLogic logic = logics;
        AdvertisementWriter writer = new AdvertisementWriter(logic);
        writer.Write(advertisement);
        _listener?.Publish("Объявление записано в файл");
        _observer.ResetToRootPath();
        _listener?.Publish("Возврат в корневую папку");
        return new AdvertisementFileSystemResult(true);
    }

    public AdvertisementFileSystemResult CreateDirectory(AdvertisementFolder folder)
    {
        _listener?.Publish("Создание папки");
        string folderPath = Path.Combine(_observer.CurrentDirectory.CurrentPath, folder.Folder);
        if (Directory.Exists(folderPath))
        {
            string message = "Ошибка: Такая папка уже существует";
            _listener?.Publish(message);
            return new AdvertisementFileSystemResult(false, message);
        }
        Directory.CreateDirectory(folderPath);
        _listener?.Publish("Папка создана");
        _observer.RefreshSubFoldersList();
        _listener?.Publish("Возврат в корневую папку");
        return new AdvertisementFileSystemResult(true);
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
