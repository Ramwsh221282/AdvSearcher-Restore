using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.Abstractions;

public interface IAdvertisementContentBuilder
{
    public Result<string> Build();
}
