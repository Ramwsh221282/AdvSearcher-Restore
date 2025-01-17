using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.FileSystem.Plugin.WritingOperations;

internal interface IAdvertisementWritingLogic
{
    void Process(Advertisement advertisement);
}
