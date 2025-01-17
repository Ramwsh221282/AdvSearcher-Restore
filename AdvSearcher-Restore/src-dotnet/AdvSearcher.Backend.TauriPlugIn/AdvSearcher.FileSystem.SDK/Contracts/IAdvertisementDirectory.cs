namespace AdvSearcher.FileSystem.SDK.Contracts;

public interface IAdvertisementDirectory
{
    public string ParentPath { get; set; }
    public string CurrentPath { get; set; }
    public IReadOnlyCollection<string> CurrentSubFolders { get; }
}
