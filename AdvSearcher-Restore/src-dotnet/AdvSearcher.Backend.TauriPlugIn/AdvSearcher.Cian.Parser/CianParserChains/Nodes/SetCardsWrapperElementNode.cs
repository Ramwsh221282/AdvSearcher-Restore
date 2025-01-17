using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Cian.Parser.CianParserChains.PipeLineComponents;
using OpenQA.Selenium;

namespace AdvSearcher.Cian.Parser.CianParserChains.Nodes;

public sealed class SetCardsWrapperElementNode : ICianParserChain
{
    private const string CardsWrapperXpath = "_93444fe79c--wrapper--W0WqH";
    private readonly IMessageListener _listener;
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardsWrapperElementNode(
        CianParserPipeLine pipeLine,
        IMessageListener listener,
        ICianParserChain? next = null
    )
    {
        _listener = listener;
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Получение карточек Циана.");
        _pipeLine.NotificationsPublisher?.Invoke("Получение карточек Циана.");
        if (_pipeLine.Provider.Instance == null)
        {
            string message = "Веб драйвер не был запущен. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        IWebElement? element = _pipeLine.Provider.Instance.FindElement(
            By.ClassName(CardsWrapperXpath)
        );
        if (element == null)
        {
            string message = "Элемент контейнер карточек не найден. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _pipeLine.SetCardsWrapperElement(new CianCardsWrapperElement(element));
        _pipeLine.NotificationsPublisher?.Invoke("Контейнер карточек проинициализирован.");
        _listener.Publish("Контейнер карточек проинициализирован.");
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
