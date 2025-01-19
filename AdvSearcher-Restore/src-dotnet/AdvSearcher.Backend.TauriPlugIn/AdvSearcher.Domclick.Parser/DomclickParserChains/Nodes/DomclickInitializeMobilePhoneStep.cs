using AdvSearcher.Application.Contracts.MessageListeners;

namespace AdvSearcher.Domclick.Parser.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeMobilePhoneStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IMessageListener _listener;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;
    private int _currentProgress;

    public DomclickInitializeMobilePhoneStep(
        DomclickParserPipeline pipeline,
        IMessageListener listener,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _listener = listener;
        Next = next;
    }

    public async Task Process()
    {
        _listener.Publish("Получение номеров телефонов");
        _pipeline.NotificationsPublisher?.Invoke("Получение номеров телефонов");
        if (!_pipeline.FetchResults.Any())
        {
            string message = "Данных каталога нет. Остановка процесса.";
            _pipeline.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        int attempts = 0;
        int limitAttempts = 10;
        _pipeline.MaxProgressPublisher?.Invoke(_pipeline.FetchResults.Count);
        _pipeline.FilterByDate();
        _pipeline.FilterByCache();
        foreach (var result in _pipeline.FetchResults)
        {
            if (attempts == limitAttempts)
            {
                string message = "Достигнуто 10 попыток в получении номера телефона. Пауза минута.";
                _pipeline.NotificationsPublisher?.Invoke(message);
                _listener.Publish(message);
                await Task.Delay(TimeSpan.FromSeconds(60));
            }
            try
            {
                using (DomclickPhoneInitializer initializer = new DomclickPhoneInitializer(result))
                {
                    await initializer.GetResearchApiToken();
                    await initializer.GetPhoneNumber();
                }
                _currentProgress++;
                _pipeline.CurrentProgressPublisher?.Invoke(_currentProgress);
                attempts++;
            }
            catch
            {
                string message = "Получена блокировка ДомКлик. Остановка процесса.";
                _pipeline.NotificationsPublisher?.Invoke(message);
                _listener.Publish(message);
                break;
            }
        }

        if (Next != null)
            await Next.Process();
    }
}
