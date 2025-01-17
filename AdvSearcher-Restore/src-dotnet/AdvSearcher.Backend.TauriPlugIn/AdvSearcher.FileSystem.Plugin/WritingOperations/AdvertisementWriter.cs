using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.FileSystem.Plugin.WritingOperations;

internal sealed class AdvertisementWriter
{
    private readonly IAdvertisementWritingLogic _logic;

    public AdvertisementWriter(IAdvertisementWritingLogic logic) => _logic = logic;

    public void Write(Advertisement advertisement)
    {
        _logic.Process(advertisement);
    }
}
