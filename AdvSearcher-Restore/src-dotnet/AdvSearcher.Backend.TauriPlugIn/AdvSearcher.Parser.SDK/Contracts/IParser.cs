using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Application.Contracts.Progress;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Parser.SDK.Contracts;

public interface IParser : IProgressable, IListenable, INotificatable
{
    Task<Result<bool>> ParseData(ServiceUrl url, List<ParserFilterOption>? options = null);
    IReadOnlyCollection<IParserResponse> Results { get; }
}
