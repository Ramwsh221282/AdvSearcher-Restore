using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Parser.SDK.Contracts;

public interface IParser
{
    Task<Result<bool>> ParseData(ServiceUrl url, List<ParserFilterOption>? options = null);
    IReadOnlyCollection<IParserResponse> Results { get; }
}
