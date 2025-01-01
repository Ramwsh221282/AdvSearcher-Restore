using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.Filters;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;

internal sealed class CreateAdvertisementResponseNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    private readonly ParserConsoleLogger _logger;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public CreateAdvertisementResponseNode(
        OkParserPipeLine pipeLine,
        IHttpClient client,
        ParserConsoleLogger logger,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = client;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Creating Ok Advertisement Responses");
        if (_pipeLine.Nodes == null)
        {
            _logger.Log("Ok Nodes were null. Stopping process.");
            return;
        }

        ParserFilter filter = new ParserFilter(_pipeLine.Options);
        foreach (var node in _pipeLine.Nodes.Nodes)
        {
            _pipeLine.InstantiateUrlBuilder(node);
            _pipeLine.InstantiateDateBuilder(node);
            if (!await IsMatchingDate(filter))
                continue;
            _pipeLine.InstantiateContentBuilder(node, _httpClient);
            _pipeLine.InstantiatePublisherBuilder(node);
            await _pipeLine.AddAdvertisementResponse(filter);
            _logger.Log("Created Ok Advertisement Response");
        }
        _httpClient.Dispose();
        _logger.Log($"Ok advertisement responses created. Count: {_pipeLine.Responses.Count}");
        if (Next != null)
        {
            _logger.Log("Processing next chain step.");
            await Next.ExecuteAsync();
        }
    }

    private async Task<bool> IsMatchingDate(ParserFilter filter)
    {
        Result<DateOnly> date = await _pipeLine.GetAdvertisementDate();
        if (date.IsFailure)
            return false;
        IParserFilterVisitor visitor = new OkDateOnlyFilterVisitor(date.Value);
        return filter.IsMatchingFilters(visitor);
    }
}
