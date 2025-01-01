using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep.Commands;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep;

internal sealed class ProcessAdvertisementsStep : IAvitoFastParserStep
{
    private readonly WebDriverProvider _driver;
    private readonly ParserConsoleLogger _logger;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public ProcessAdvertisementsStep(
        AvitoFastParserPipeline pipeLine,
        ParserConsoleLogger logger,
        WebDriverProvider driver,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeLine;
        Next = next;
        _driver = driver;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        _logger.Log("Start processing advertisements");
        if (_driver.Instance == null)
        {
            _logger.Log("Driver instance is null. Stopping process");
            return;
        }
        if (Pipeline.Total == 0)
        {
            _logger.Log("No advertisements were found. Stopping process");
            return;
        }

        Pipeline.FilterByDate(_logger);
        Pipeline.FilterByCache();
        foreach (AvitoAdvertisement advertisement in Pipeline)
        {
            await new NavigateOnAdvertisementPage(advertisement).ExecuteAsync(_driver);
            bool isHomeOwner = await new IsHomeOwnerQuery().ExecuteAsync(_driver);
            if (!isHomeOwner)
            {
                advertisement.IsAgent = true;
                _logger.Log($"Advertisement {advertisement.Id} is not by homeowner. Skipping.");
            }
            await new ProcessAdvertisementDescription(advertisement).ExecuteAsync(_driver);
            await new ProcessAdvertisementPublisher(advertisement).ExecuteAsync(_driver);
        }
        Pipeline.CleanFromAgents();
        _driver.Dispose();
        if (Next != null)
        {
            _logger.Log("Processing next step");
            await Next.ProcessAsync();
        }
    }
}
