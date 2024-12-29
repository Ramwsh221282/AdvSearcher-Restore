using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.AvitoParserChain;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoParser : IParser
{
    private readonly List<IParserResponse> _results = [];
    private readonly IAvitoChainNode _node;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public AvitoParser(IAvitoChainNode node) => _node = node;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        _node.Pipeline.SetServiceUrl(url);
        await _node.ExecuteAsync();
        _results.AddRange(_node.Pipeline.Responses);
        return true;
    }
}
