using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.FileSystem.Plugin.WritingOperations.WritingLogics;

internal sealed class CompositeAdvertisementWritingLogic : IAdvertisementWritingLogic
{
    private readonly List<IAdvertisementWritingLogic> _logics = [];

    public void Process(Advertisement advertisement)
    {
        foreach (var logic in _logics)
        {
            logic.Process(advertisement);
        }
    }

    public void AddLogic(IAdvertisementWritingLogic logic) => _logics.Add(logic);
}
