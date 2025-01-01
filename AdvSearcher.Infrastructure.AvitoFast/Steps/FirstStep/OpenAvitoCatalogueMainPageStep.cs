using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FirstStep;

internal sealed class OpenAvitoCatalogueMainPageStep : IAvitoFastParserStep
{
    private readonly ParserConsoleLogger _logger;
    private readonly WebDriverProvider _driverProvider;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public OpenAvitoCatalogueMainPageStep(
        AvitoFastParserPipeline pipeline,
        ParserConsoleLogger logger,
        WebDriverProvider driverProvider,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeline;
        Next = next;
        _logger = logger;
        _driverProvider = driverProvider;
    }

    public async Task ProcessAsync()
    {
        _logger.Log("Opening avito catalogue page step");
        if (Pipeline.Url == null)
        {
            _logger.Log("Catalogue page url was not provided. Stopping process.");
            return;
        }
        _logger.Log("Creating web driver instance");
        _driverProvider.InstantiateNewWebDriver();
        _logger.Log("Navigating on avito catalogue page");
        await new NavigateOnPageCommand(Pipeline.Url.Value.Value).ExecuteAsync(_driverProvider);
        _logger.Log("Sleeping for cookies initialization");
        await Task.Delay(TimeSpan.FromSeconds(30));
        if (Next != null)
        {
            _logger.Log("Processing next step");
            await Next.ProcessAsync();
        }
    }
}
