namespace AdvSearcher.Publishing.SDK.Models;

public sealed record AdvertisementFileResponse(
    string Path,
    string DisplayName,
    List<string> Photos,
    bool IsMarked = false
)
{
    public const string TextFilePath = "Text.txt";
}
