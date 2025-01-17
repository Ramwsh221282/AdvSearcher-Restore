namespace AdvSearcher.Publishing.SDK.Models;

internal sealed class AdvertisementFile
{
    private const string PhotoFolder = "Photos";
    private static readonly string[] PhotoExtensionConstraints = [".png", ".jpg"];

    public string RootDirectoryName { get; }
    public string FolderPath { get; }
    public string TextFilePath { get; }
    public List<string> PhotoPaths { get; } = [];

    public AdvertisementFile(string rootDirectoryName, string folderPath, string textFilePath)
    {
        RootDirectoryName = rootDirectoryName;
        FolderPath = folderPath;
        TextFilePath = textFilePath;
        string photoPath = Path.Combine(FolderPath, PhotoFolder);
        if (Directory.Exists(photoPath))
        {
            string[] photos = Directory.GetFiles(photoPath);
            foreach (var photo in photos)
            {
                if (
                    PhotoExtensionConstraints.Any(ext =>
                        photo.Contains(ext, StringComparison.OrdinalIgnoreCase)
                    )
                )
                    PhotoPaths.Add(photo);
            }
        }
    }

    public AdvertisementFileResponse ConvertToResponse()
    {
        ReadOnlySpan<string> pathParts = FolderPath.Split('\\');
        string displayName = pathParts[^1];
        return new AdvertisementFileResponse(FolderPath, displayName, PhotoPaths);
    }
}
