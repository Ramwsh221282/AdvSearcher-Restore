using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin;

public sealed class AdvertisementDirectory : IAdvertisementDirectory
{
    private string _parentPath = string.Empty;
    private string _currentPath = string.Empty;
    private string[] _subFolders = [];

    public string ParentPath
    {
        get { return _parentPath; }
        set { _parentPath = value; }
    }

    public string CurrentPath
    {
        get => _currentPath;
        set
        {
            _currentPath = value;
            FetchSubfolders();
        }
    }

    public IReadOnlyCollection<string> CurrentSubFolders => _subFolders;

    public void FetchSubfolders()
    {
        _subFolders = Directory.GetDirectories(_currentPath);
        _subFolders = _subFolders.Where(folder => !IsAdvertisementFileFolder(folder)).ToArray();
    }

    private bool IsAdvertisementFileFolder(string folder)
    {
        string[] files = Directory.GetFiles(folder);
        if (files.Any(f => f.Contains(".txt")))
            return true;
        return false;
    }
}
