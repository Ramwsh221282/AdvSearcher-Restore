using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep.Commands;
using AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep.Models;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep;

internal sealed class FetchAvitoCatalogueStep : IAvitoFastParserStep
{
    private readonly WebDriverProvider _driverProvider;
    private readonly ParserConsoleLogger _logger;
    private int page;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public FetchAvitoCatalogueStep(
        AvitoFastParserPipeline pipeline,
        WebDriverProvider driverProvider,
        ParserConsoleLogger logger,
        IAvitoFastParserStep? next = null
    )
    {
        _driverProvider = driverProvider;
        Pipeline = pipeline;
        _logger = logger;
        Next = next;
    }

    public async Task ProcessAsync()
    {
        _logger.Log("Fetching avito catalogue step");
        if (_driverProvider.Instance == null)
        {
            _logger.Log("Driver provider was not instantiated. Stopping process");
            return;
        }
        _logger.Log("Starting fetching");
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
                    _logger.Log("Navigation compeleted. Delay 5 seconds");
                    Result<string> jsonData = await new ExtractPageJsonQuery().ExecuteAsync(
                        _driverProvider
                    );
                    if (jsonData.IsFailure)
                    {
                        _logger.Log("Json data of the page was empty. Skipping page.");
                        continue;
                    }
                    Result<CatalogueItemsJsonContainer> container =
                        CatalogueItemsJsonContainer.Create(JObject.Parse(jsonData.Value));
                    if (container.IsFailure)
                    {
                        _logger.Log("Invalid json data. Skipping page.");
                    }
                    CatalogueItemJson[] items = container.Value.CreateCatalogueItemsArray();
                    AppendItemsToPipeLine(items);
                    _logger.Log("Json data parsed.");
                    page++;
                    isIterationSuccess = true;
                }
                catch
                {
                    _logger.Log("Failed to parse avito jsons. Retrying.");
                }
            }
        }
        if (Next != null)
        {
            _logger.Log("Processing next step");
            await Next.ProcessAsync();
        }
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
