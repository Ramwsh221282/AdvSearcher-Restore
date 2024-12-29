using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickResultsInitializerStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickResultsInitializerStep(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        if (!_pipeline.FetchResults.Any())
            return;
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
        if (Next != null)
            await Next.Process();
    }
}
