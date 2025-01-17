using AdvSearcher.FileSystem.Plugin.Operations;
using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin;

public sealed class DirectoryObserver
{
    private static readonly string RootFilePath = @$"{Environment.CurrentDirectory}\Data";
    private readonly AdvertisementDirectory _observable = new AdvertisementDirectory();
    private IAdvertisementFileActionStrategy _strategy = new NoActionsAdvertisementFileStrategy();
    public IAdvertisementDirectory CurrentDirectory => _observable;

    public DirectoryObserver()
    {
        if (!Directory.Exists(RootFilePath))
            Directory.CreateDirectory(RootFilePath);
        _observable.CurrentPath = RootFilePath;
        _observable.ParentPath = RootFilePath;
    }

    public void RefreshSubFoldersList() => _observable.FetchSubfolders(); // refreshes list of subfolders in current directory of observable

    public void MoveToNextSubfolder(string folderPath) // moves observable to its subfolder if subfolder exists
    {
        _strategy = new MoveToSubfolderStrategy(folderPath);
        _strategy.Perform(_observable);
    }

    public void MoveToPreviousFolder() // moves observable back
    {
        _strategy = new MoveToParentStrategy();
        _strategy.Perform(_observable);
    }

    public void ResetToRootPath() // moves observable to root path (Data).
    {
        _observable.CurrentPath = RootFilePath;
        _observable.ParentPath = RootFilePath;
    }

    public IReadOnlyCollection<string> GetSubfolderNames() // gets sub folder names from current observable folder
    {
        if (!_observable.CurrentSubFolders.Any())
            return [];
        string[] data = new string[_observable.CurrentSubFolders.Count];
        int index = 0;
        foreach (string subfolder in _observable.CurrentSubFolders)
        {
            ReadOnlySpan<string> details = subfolder.Split('\\');
            data[index] = details[^1];
            index++;
        }
        return data;
    }
}
