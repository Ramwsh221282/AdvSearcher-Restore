using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Parser.SDK.Contracts;

public interface IParser
{
    Task<Result<bool>> ParseData(ServiceUrl url);
    IReadOnlyCollection<IParserResponse> Results { get; }
}
