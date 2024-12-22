using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Abstractions.Parsers;

public abstract record ParserService;

public sealed record VkParserService : ParserService;

public sealed record OkParserService : ParserService;

public sealed record CianParserService : ParserService;

public sealed record AvitoParserService : ParserService;

public sealed record DomclickParserService : ParserService;

public interface IParser<TParserService>
    where TParserService : ParserService
{
    Task<Result<bool>> ParseData(ServiceUrl url);
    IReadOnlyCollection<IParserResponse> Results { get; }
}
