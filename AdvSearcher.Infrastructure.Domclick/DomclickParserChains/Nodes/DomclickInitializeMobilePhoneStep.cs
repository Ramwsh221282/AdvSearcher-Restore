namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeMobilePhoneStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickInitializeMobilePhoneStep(
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
            try
            {
                using (DomclickPhoneInitializer initializer = new DomclickPhoneInitializer(result))
                {
                    Console.WriteLine($"Domclick. Requesting for ${result.Id}");
                    await initializer.GetResearchApiToken();
                    await initializer.GetPhoneNumber();
                    Thread.Sleep(5000);
                }
            }
            catch
            {
                Console.WriteLine(
                    $"Failed to initialize Domclick advertisement. Requesting for ${result.Id}"
                );
            }
        }
        if (Next != null)
            await Next.Process();
    }
}
