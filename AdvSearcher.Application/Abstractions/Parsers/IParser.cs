using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Abstractions.Parsers;

public interface IParser<TService>
    where TService : class
{
    Task<Result<bool>> ParseData(ServiceUrl url);
    IReadOnlyCollection<IParserResponse> Results { get; }
}
