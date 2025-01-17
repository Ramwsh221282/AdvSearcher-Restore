namespace AdvSearcher.Publishing.SDK.Models;

internal sealed class AdvertisementDirectory
{
    public string FolderName { get; }
    private readonly List<AdvertisementDirectory> SubFolders = [];
    public List<AdvertisementFile> Files { get; } = [];

    public AdvertisementDirectory(string folderName) => FolderName = folderName;

    public void FetchInside()
    {
        string[] directories = Directory.GetDirectories(FolderName);
        foreach (string directory in directories)
        {
            string textFilePath = Path.Combine(directory, "Text.txt");
            if (File.Exists(textFilePath))
            {
                AdvertisementFile file = new AdvertisementFile(FolderName, directory, textFilePath);
                Files.Add(file);
                continue;
            }
            SubFolders.Add(new AdvertisementDirectory(directory));
        }

        foreach (var folder in SubFolders)
        {
            folder.FetchInside();
        }
    }

    public List<AdvertisementDirectory> GetDirectoriesWithAdvertisements()
    {
        List<AdvertisementDirectory> directories = [];
        if (Files.Count > 0)
            directories.Add(this);
        foreach (var folder in SubFolders)
            directories.AddRange(folder.GetDirectoriesWithAdvertisements());
        return directories;
    }

    public AdvertisementDirectoryResponse ConvertToResponse()
    {
        ReadOnlySpan<string> parts = FolderName.Split('\\');
        string displayName = parts[^1];
        List<AdvertisementFileResponse> files = [];
        foreach (var file in Files)
        {
            files.Add(file.ConvertToResponse());
        }
        return new AdvertisementDirectoryResponse(FolderName, displayName, files);
    }
}
