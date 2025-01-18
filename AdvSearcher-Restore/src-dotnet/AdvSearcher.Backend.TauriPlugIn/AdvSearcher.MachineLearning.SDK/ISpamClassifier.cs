using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.MachineLearning.SDK;

public interface ISpamClassifier : IDisposable
{
    bool IsSpam(Advertisement advertisement);
}
