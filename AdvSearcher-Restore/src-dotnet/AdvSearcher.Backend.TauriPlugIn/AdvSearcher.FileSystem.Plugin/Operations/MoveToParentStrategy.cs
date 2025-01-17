namespace AdvSearcher.FileSystem.Plugin.Operations;

public sealed class MoveToParentStrategy : IAdvertisementFileActionStrategy
{
    public void Perform(AdvertisementDirectory directory)
    {
        if (IsAtRootDirectory(directory) || IsParentRoot(directory))
        {
            string parent = directory.ParentPath;
            directory.CurrentPath = parent;
            directory.ParentPath = parent;
            return;
        }
        string newParent = GetNewParentDirectory(directory);
        directory.CurrentPath = directory.ParentPath;
        directory.ParentPath = newParent;
    }

    private bool IsAtRootDirectory(AdvertisementDirectory directory)
    {
        ReadOnlySpan<string> pathDetails = directory.CurrentPath.Split('\\');
        return pathDetails[^1] == "Data";
    }

    private bool IsParentRoot(AdvertisementDirectory directory)
    {
        ReadOnlySpan<string> pathDetails = directory.ParentPath.Split('\\');
        return pathDetails[^1] == "Data";
    }

    private string GetNewParentDirectory(AdvertisementDirectory directory) =>
        directory.ParentPath.Substring(0, directory.ParentPath.LastIndexOf('\\'));
}
