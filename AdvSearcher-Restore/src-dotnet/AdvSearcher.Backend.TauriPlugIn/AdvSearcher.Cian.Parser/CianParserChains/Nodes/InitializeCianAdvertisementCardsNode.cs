using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Cian.Parser.Materials.CianComponents;

namespace AdvSearcher.Cian.Parser.CianParserChains.Nodes;

public sealed class InitializeCianAdvertisementCardsNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly IMessageListener _listener;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public InitializeCianAdvertisementCardsNode(
        CianParserPipeLine pipeLine,
        IMessageListener listener,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Инициализация карточек циана.");
        _pipeLine.NotificationsPublisher?.Invoke("Инициализация карточек циана.");
        if (_pipeLine.CardElements == null)
        {
            string message = "Данные каталога не были получены. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
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
        }
        _pipeLine.Provider.Dispose();
        _pipeLine.SetCianAdvertisementCards(cards.ToArray());
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
