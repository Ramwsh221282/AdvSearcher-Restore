using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Avito.Parser.Steps.FourthStep.Converters;
using AdvSearcher.Avito.Parser.Steps.FourthStep.Models;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep;

public sealed class CreateResponsesStep : IAvitoFastParserStep
{
    private readonly IMessageListener _listener;
    private readonly AvitoDateConverter _converter;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public CreateResponsesStep(
        AvitoFastParserPipeline pipeLine,
        IMessageListener listener,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeLine;
        _listener = listener;
        Next = next;
        _converter = new AvitoDateConverter();
    }

    public async Task ProcessAsync()
    {
        Pipeline.NotificationsPublisher?.Invoke("Создание результатов парсинга Авито.");
        _listener.Publish("Создание результатов парсинга Авито.");
        if (Pipeline.Total == 0)
        {
            string message = "Нет данных каталога Авито. Остановка процесса.";
            Pipeline.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        foreach (AvitoAdvertisement advertisement in Pipeline)
        {
            Result<IParsedAdvertisement> ad = AvitoResponseAdvertisement.Create(
                advertisement,
                _converter
            );
            Result<IParsedPublisher> publisher = AvitoResponsePublisher.Create(advertisement);
            if (ad.IsFailure || publisher.IsFailure)
                continue;
            IParsedAttachment[] attachments = AvitoResponseAttachment.Create(advertisement);
            IParserResponse response = new AvitoResponse(ad.Value, attachments, publisher.Value);
            Pipeline.Results.Add(response);
        }
        Pipeline.FilterByPublishers();
        if (Next != null)
            await Next.ProcessAsync();
    }
}
