using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace Advsearcher.Infrastructure.VKParser;

internal sealed class VkParser : IParser
{
    private readonly IVkParserNode _chainedNode;
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public VkParser(IVkParserNode chainedNode) => _chainedNode = chainedNode;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        if (options != null)
            _chainedNode.PipeLine.FilterOptions = options;
        _chainedNode.PipeLine.SetServiceUrl(url);
        await _chainedNode.ExecuteAsync();
        _results.AddRange(_chainedNode.PipeLine.Responses);
        return true;
    }
}
