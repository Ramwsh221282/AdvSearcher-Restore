namespace AdvSearcher.FileSystem.Plugin.Operations;

public sealed class MoveToSubfolderStrategy : IAdvertisementFileActionStrategy
{
    private readonly string _subFolderName;

    public MoveToSubfolderStrategy(string subfolderName) => _subFolderName = subfolderName;

    public void Perform(AdvertisementDirectory directory)
    {
        if (IsDirectoryExists(directory, out string searchedDirectory))
        {
            string parent = directory.CurrentPath;
            directory.CurrentPath = searchedDirectory;
            directory.ParentPath = parent;
        }
    }

    private bool IsDirectoryExists(AdvertisementDirectory directory, out string searchedDirectory)
    {
        searchedDirectory = string.Empty;
        foreach (string path in directory.CurrentSubFolders)
        {
            ReadOnlySpan<string> pathDetails = path.Split('\\');
            if (pathDetails[^1] == _subFolderName)
            {
                searchedDirectory = path;
                return true;
            }
        }
        return false;
    }
}
