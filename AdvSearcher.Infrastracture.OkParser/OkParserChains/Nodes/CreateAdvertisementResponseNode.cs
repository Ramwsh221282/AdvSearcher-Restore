using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;

internal sealed class CreateAdvertisementResponseNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public CreateAdvertisementResponseNode(
        OkParserPipeLine pipeLine,
        IHttpClient client,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = client;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.Nodes == null)
            return;
        foreach (var node in _pipeLine.Nodes.Nodes)
        {
            _pipeLine.InstantiateUrlBuilder(node);
            _pipeLine.InstantiateDateBuilder(node);
            _pipeLine.InstantiateContentBuilder(node, _httpClient);
            _pipeLine.InstantiatePublisherBuilder(node);
            await _pipeLine.AddAdvertisementResponse();
        }
        _httpClient.Dispose();
    }
}
