namespace AdvSearcher.Publishing.SDK.Models;

public sealed record AdvertisementDirectoryResponse(
    string Path,
    string DisplayName,
    List<AdvertisementFileResponse> Files,
    bool IsMarked = false
);
