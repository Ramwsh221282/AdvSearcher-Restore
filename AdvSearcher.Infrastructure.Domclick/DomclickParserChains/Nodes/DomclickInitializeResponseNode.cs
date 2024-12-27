using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeResponseNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickInitializeResponseNode(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        if (_pipeline.DomclickItems == null || _pipeline.DomclickItems.Length == 0)
            return;
        foreach (var item in _pipeline.DomclickItems)
        {
            Result<IParserResponse> response = CreateParserResponse(item);
            if (response.IsFailure)
                continue;
            _pipeline.AddParserResponse(response.Value);
        }
        if (Next != null)
            await Next.Process();
    }

    private Result<IParserResponse> CreateParserResponse(DomclickFetchResult fetchResult)
    {
        Result<IParsedAdvertisement> advertisement = DomclickAdvertisement.Create(fetchResult);
        if (advertisement.IsFailure)
            return advertisement.Error;
        Result<IParsedPublisher> publisher = DomclickPublisher.Create(fetchResult);
        if (publisher.IsFailure)
            return advertisement.Error;
        IParsedAttachment[] attachments = DomclickAttachment.Create(fetchResult);
        return new DomclickParserResponse(advertisement.Value, attachments, publisher.Value);
    }
}
