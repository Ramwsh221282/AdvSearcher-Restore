using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickResultsInitializerStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly ParserConsoleLogger _logger;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickResultsInitializerStep(
        DomclickParserPipeline pipeline,
        ParserConsoleLogger logger,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _logger = logger;
        Next = next;
    }

    public async Task Process()
    {
        _logger.Log("Domclick. Creating parser responses");
        if (!_pipeline.FetchResults.Any())
        {
            _logger.Log("No domclick results found");
            return;
        }
        foreach (var result in _pipeline.FetchResults)
        {
            Result<IParsedAdvertisement> advertisement = DomclickAdvertisement.Create(result);
            Result<IParsedPublisher> publisher = DomclickPublisher.Create(result);
            if (advertisement.IsFailure || publisher.IsFailure)
            {
                _logger.Log("Failed to create advertisement");
                continue;
            }
            IParsedAttachment[] attachments = DomclickAttachment.Create(result);
            IParserResponse response = new DomclickParserResponse(
                advertisement.Value,
                attachments,
                publisher.Value
            );
            _pipeline.AddParserResponse(response);
            _logger.Log("Successfully created parser response");
        }
        if (Next != null)
        {
            _logger.Log("Processing next domclick parser chain");
            await Next.Process();
        }
    }
}
