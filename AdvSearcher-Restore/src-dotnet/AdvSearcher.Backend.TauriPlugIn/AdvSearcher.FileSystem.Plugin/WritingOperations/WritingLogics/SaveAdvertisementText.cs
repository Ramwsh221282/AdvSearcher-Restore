using System.Text;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.FileSystem.SDK.Contracts;

namespace AdvSearcher.FileSystem.Plugin.WritingOperations.WritingLogics;

internal sealed class SaveAdvertisementText : IAdvertisementWritingLogic
{
    private const string TextFileName = "Text.txt";
    private readonly string _advertisementDirectory;
    private IAdvertisementDirectory _directory;

    public SaveAdvertisementText(
        IAdvertisementDirectory directory,
        string advertisementDirectory
    ) => (_directory, _advertisementDirectory) = (directory, advertisementDirectory);

    public void Process(Advertisement advertisement)
    {
        Console.WriteLine("Creating path");
        string path = Path.Combine(_directory.CurrentPath, _advertisementDirectory, TextFileName);
        Console.WriteLine($"Path created: {path}");
        Console.WriteLine("Creating text file");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(advertisement.Content.Content);
        stringBuilder.AppendLine(advertisement.Publisher!.Data.Value);
        stringBuilder.AppendLine(advertisement.Date.Value.ToShortDateString());
        stringBuilder.AppendLine(advertisement.Url.Url);
        File.WriteAllText(path, stringBuilder.ToString());
        Console.WriteLine("Text file created");
    }
}
