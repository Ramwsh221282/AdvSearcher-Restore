using AdvSearcher.Parser.SDK;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeMobilePhoneStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly ParserConsoleLogger _logger;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickInitializeMobilePhoneStep(
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
        _logger.Log("Starting fetching domclick mobile phones");
        if (!_pipeline.FetchResults.Any())
        {
            _logger.Log("No domclick items were found");
            return;
        }
        int attempts = 0;
        int limitAttempts = 5;
        foreach (var result in _pipeline.FetchResults)
        {
            if (attempts == limitAttempts)
            {
                _logger.Log("Stopping process since Domclick is blocking requests.");
                break;
            }
            try
            {
                using (DomclickPhoneInitializer initializer = new DomclickPhoneInitializer(result))
                {
                    _logger.Log($"Domclick. Requesting for {result.Id}");
                    await initializer.GetResearchApiToken();
                    await initializer.GetPhoneNumber();
                }
            }
            catch
            {
                _logger.Log(
                    $"Failed to initialize Domclick advertisement. Requesting for {result.Id}"
                );
                limitAttempts++;
            }
        }

        if (Next != null)
        {
            _logger.Log("Starting next step in chain");
            await Next.Process();
        }
    }
}
