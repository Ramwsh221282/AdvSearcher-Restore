using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.OkParserChains;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkParser : IParser
{
    private readonly List<IParserResponse> _results = [];
    private readonly IOkParserChain _chain;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public OkParser(IOkParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        _chain.PipeLine.SetServiceUrl(url);
        if (options != null)
            _chain.PipeLine.Options = options;
        await _chain.ExecuteAsync();
        _results.AddRange(_chain.PipeLine.Responses);
        return true;
    }
}
