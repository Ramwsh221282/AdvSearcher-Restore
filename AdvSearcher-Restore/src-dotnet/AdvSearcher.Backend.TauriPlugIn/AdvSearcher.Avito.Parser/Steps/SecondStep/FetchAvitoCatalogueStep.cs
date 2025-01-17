using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Avito.Parser.Steps.SecondStep.Commands;
using AdvSearcher.Avito.Parser.Steps.SecondStep.Models;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Avito.Parser.Steps.SecondStep;

public sealed class FetchAvitoCatalogueStep : IAvitoFastParserStep
{
    private readonly WebDriverProvider _driverProvider;
    private readonly IMessageListener _listener;
    private int page;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public FetchAvitoCatalogueStep(
        AvitoFastParserPipeline pipeline,
        WebDriverProvider driverProvider,
        IMessageListener listener,
        IAvitoFastParserStep? next = null
    )
    {
        _driverProvider = driverProvider;
        Pipeline = pipeline;
        _listener = listener;
        Next = next;
    }

    public async Task ProcessAsync()
    {
        Pipeline.NotificationsPublisher?.Invoke("Начало парсинга каталога авито.");
        _listener.Publish("Начало парсинга каталога авито.");
        if (_driverProvider.Instance == null)
        {
            string message = "Веб драйвер не был запущен. Остановка процесса.";
            Pipeline.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        while (page != 7)
        {
            bool isIterationSuccess = false;
            while (!isIterationSuccess)
            {
                try
                {
                    string url = CreateUrl();
                    await new NavigateOnPageCommand(url).ExecuteAsync(_driverProvider);
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    await new ScrollToBottomCommand().ExecuteAsync(_driverProvider);
                    await new ScrollToTopCommand().ExecuteAsync(_driverProvider);
                    Result<string> jsonData = await new ExtractPageJsonQuery().ExecuteAsync(
                        _driverProvider
                    );
                    if (jsonData.IsFailure)
                        continue;
                    Result<CatalogueItemsJsonContainer> container =
                        CatalogueItemsJsonContainer.Create(JObject.Parse(jsonData.Value));
                    if (container.IsFailure)
                        continue;
                    CatalogueItemJson[] items = container.Value.CreateCatalogueItemsArray();
                    AppendItemsToPipeLine(items);
                    page++;
                    isIterationSuccess = true;
                }
                catch
                {
                    Pipeline.NotificationsPublisher?.Invoke(
                        "Ошибка в парсинге Авито. Рестарт попытки."
                    );
                    _listener.Publish("Ошибка в парсинге Авито. Рестарт попытки.");
                }
            }
        }
        if (Next != null)
            await Next.ProcessAsync();
    }

    private void AppendItemsToPipeLine(CatalogueItemJson[] items)
    {
        foreach (CatalogueItemJson item in items)
        {
            Pipeline.AddAdvertisement(item);
        }
    }

    private string CreateUrl()
    {
        return $"https://m.avito.ru/api/11/items?key=af0deccbgcgidddjgnvljitntccdduijhdinfgjgfjir&categoryId=24&locationId=635340&params[201]=1059&context=H4sIAAAAAAAA_wFCAL3_YToxOntzOjU6Inhfc2d0IjtzOjQwOiIxYTY3NGI0N2M5MWNmNDkyOGFhN2NlYTk4NWJkM2I3NDNlZjQ4OWE2Ijt9LQEaq0IAAAA&page={page}&lastStamp=1735565580&layoutRange=narrow&presentationType=serp";
    }
}
