using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.OkParserChains;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkParser : IParser<OkParserService>
{
    private readonly List<IParserResponse> _results = [];
    private readonly IOkParserChain _chain;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public OkParser(IOkParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        _chain.PipeLine.SetServiceUrl(url);
        await _chain.ExecuteAsync();
        _results.AddRange(_chain.PipeLine.Responses);
        return true;
    }
}
