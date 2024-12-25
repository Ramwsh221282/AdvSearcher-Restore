using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory;
using AdvSearcher.Infrastracture.OkParser.Utils.Materials;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkParser(OkHtmlExtractor extractor, OkAdvertisementsFactory factory)
    : IParser<OkParserService>
{
    private readonly List<IParserResponse> _results = [];

    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        Result<string> html = await extractor.Extract(url);
        OkTopicNodes nodes = new OkTopicNodes(html);
        List<IParserResponse> responses = await factory.Construct(url, nodes);
        _results.AddRange(responses);
        return true;
    }
}
