using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

namespace AdvSearcher.Avito.Parser.Steps.FirstStep;

public sealed class OpenAvitoCatalogueMainPageStep : IAvitoFastParserStep
{
    private readonly IMessageListener _listener;
    private readonly WebDriverProvider _driverProvider;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public OpenAvitoCatalogueMainPageStep(
        AvitoFastParserPipeline pipeline,
        IMessageListener listener,
        WebDriverProvider driverProvider,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeline;
        Next = next;
        _listener = listener;
        _driverProvider = driverProvider;
    }

    public async Task ProcessAsync()
    {
        Pipeline.NotificationsPublisher?.Invoke("Открытие каталога Авито.");
        _listener.Publish("Открытие каталога Авито.");
        if (Pipeline.Url == null)
        {
            string message = "Ссылка на каталог авито не указана. Остановка процесса.";
            Pipeline.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        Pipeline.NotificationsPublisher?.Invoke("Запуск веб-драйвера.");
        _listener.Publish("Запуск веб-драйвера.");
        _driverProvider.InstantiateNewWebDriver();
        await new NavigateOnPageCommand(Pipeline.Url.Value.Value).ExecuteAsync(_driverProvider);
        Pipeline.NotificationsPublisher?.Invoke("Пауза для инициализации cookie.");
        _listener.Publish("Пауза для инициализации cookie.");
        await Task.Delay(TimeSpan.FromSeconds(30));
        if (Next != null)
            await Next.ProcessAsync();
    }
}
