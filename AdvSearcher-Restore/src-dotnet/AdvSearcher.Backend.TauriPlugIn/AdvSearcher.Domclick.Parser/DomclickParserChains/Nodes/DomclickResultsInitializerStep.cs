using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;
using AdvSearcher.Domclick.Parser.InternalModels.DomclickParserResults;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Domclick.Parser.DomclickParserChains.Nodes;

internal sealed class DomclickResultsInitializerStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IMessageListener _listener;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickResultsInitializerStep(
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
        _listener.Publish("Создание результатов парсинга ДомКлик.");
        _pipeline.NotificationsPublisher?.Invoke("Создание результатов парсинга ДомКлик.");
        if (!_pipeline.FetchResults.Any())
        {
            string message = "Данные каталога не найдены. Остановка процесса.";
            _pipeline.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        foreach (var result in _pipeline.FetchResults)
        {
            Result<IParsedAdvertisement> advertisement = DomclickAdvertisement.Create(result);
            Result<IParsedPublisher> publisher = DomclickPublisher.Create(result);
            if (advertisement.IsFailure || publisher.IsFailure)
                continue;
            IParsedAttachment[] attachments = DomclickAttachment.Create(result);
            IParserResponse response = new DomclickParserResponse(
                advertisement.Value,
                attachments,
                publisher.Value
            );
            _pipeline.AddParserResponse(response);
        }
        _pipeline.FilterByPublishers();
        if (Next != null)
            await Next.Process();
    }
}
