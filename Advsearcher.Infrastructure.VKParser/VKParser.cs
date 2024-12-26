using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain;
using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser;

internal sealed class VkParser : IParser<VkParserService>
{
    private readonly IVkParserNode _chainedNode;
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public VkParser(IVkParserNode chainedNode) => _chainedNode = chainedNode;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        _chainedNode.PipeLine.SetServiceUrl(url);
        await _chainedNode.ExecuteAsync();
        _results.AddRange(_chainedNode.PipeLine.Responses);
        return true;
    }
}
