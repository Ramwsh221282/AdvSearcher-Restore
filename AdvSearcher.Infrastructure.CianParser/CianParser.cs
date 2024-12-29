using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.CianParser.CianParserChains;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.CianParser;

internal sealed class CianParser : IParser
{
    private readonly List<IParserResponse> _results = [];
    private readonly ICianParserChain _chain;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public CianParser(ICianParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        _chain.PipeLine.SetServiceUrl(url);
        await _chain.ExecuteAsync();
        _results.AddRange(_chain.PipeLine.Responses);
        return true;
    }
}
