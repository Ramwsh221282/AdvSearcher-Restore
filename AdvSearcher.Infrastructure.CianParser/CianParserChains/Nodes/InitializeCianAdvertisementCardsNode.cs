using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;
using AdvSearcher.Parser.SDK;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class InitializeCianAdvertisementCardsNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public InitializeCianAdvertisementCardsNode(
        CianParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Initializing Advertisement Cards node");
        if (_pipeLine.CardElements == null)
        {
            _logger.Log("No card elements found");
            return;
        }

        List<CianAdvertisementCard> cards = new();
        foreach (var item in _pipeLine.CardElements)
        {
            CianAdvertisementCard card = new CianAdvertisementCard(item.Element);
            if (!card.IsHomeowner())
                continue;
            card.InitializeMobilePhone();
            card.InitializeContent();
            card.InitializeSourceUrl();
            card.InitializeId();
            card.InitializeDate();
            card.InitializePhotos();
            cards.Add(card);
            _logger.Log("Card added");
        }
        _pipeLine.Provider.Dispose();
        _logger.Log("Web driver instance disposed");
        _pipeLine.SetCianAdvertisementCards(cards.ToArray());
        _logger.Log("Cian advertisement cards were instantiated");
        if (Next != null)
        {
            _logger.Log("Processing next step in chain");
            await Next.ExecuteAsync();
        }
    }
}
