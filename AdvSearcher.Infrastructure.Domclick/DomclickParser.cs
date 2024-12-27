using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick;

internal sealed class DomclickParser : IParser<DomclickParserService>
{
    private readonly IDomclickParserChain _chain;
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public DomclickParser(IDomclickParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(ServiceUrl url = null!)
    {
        await _chain.Process();
        _results.AddRange(_chain.Pipeline.Responses);
        return true;
    }
}
