using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class InitializeCianAdvertisementCardsNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public InitializeCianAdvertisementCardsNode(
        CianParserPipeLine pipeLine,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.CardElements == null)
            return;

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
        }
        _pipeLine.Provider.Dispose();
        _pipeLine.SetCianAdvertisementCards(cards.ToArray());
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
