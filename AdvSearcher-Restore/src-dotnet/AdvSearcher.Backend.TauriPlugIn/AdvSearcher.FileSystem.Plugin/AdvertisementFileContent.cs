using System.Text;
using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin;

public sealed class AdvertisementFileContent
{
    private const string TextFileName = "Text.txt";
    private const string PhotosFolderName = "Photos";

    private string _text = string.Empty;
    private List<string> _images = [];

    public string Text => _text;
    public IReadOnlyCollection<string> Images => _images;

    public AdvertisementFileContent(IAdvertisementDirectory directory)
    {
        InstantiateTextFile(directory);
        InstantiatePhotos(directory);
    }

    private void InstantiateTextFile(IAdvertisementDirectory directory)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(directory.CurrentPath);
        stringBuilder.Append('\\');
        stringBuilder.Append(TextFileName);
        if (File.Exists(stringBuilder.ToString()))
            _text = File.ReadAllText(stringBuilder.ToString());
    }

    private void InstantiatePhotos(IAdvertisementDirectory directory)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(directory.CurrentPath);
        stringBuilder.Append('\\');
        stringBuilder.Append(PhotosFolderName);
        if (!Directory.Exists(stringBuilder.ToString()))
            return;
        ReadOnlySpan<string> files = Directory.GetFiles(stringBuilder.ToString());
        foreach (string file in files)
        {
            if (
                file.Contains(".png", StringComparison.OrdinalIgnoreCase)
                || file.Contains(".jpg", StringComparison.OrdinalIgnoreCase)
            )
                _images.Add(file);
        }
    }
}
