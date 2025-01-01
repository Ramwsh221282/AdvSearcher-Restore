using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.Domclick;

internal sealed class DomclickParser : IParser
{
    private readonly IDomclickParserChain _chain;
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public DomclickParser(IDomclickParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url = null!,
        List<ParserFilterOption>? options = null
    )
    {
        await _chain.Process();
        if (options != null)
            _chain.Pipeline.Options = options;
        _results.AddRange(_chain.Pipeline.Responses);
        return true;
    }
}
