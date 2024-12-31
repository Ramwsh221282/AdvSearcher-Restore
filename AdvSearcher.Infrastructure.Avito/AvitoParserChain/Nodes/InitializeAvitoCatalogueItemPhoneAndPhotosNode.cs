using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class InitializeAvitoCatalogueItemPhoneAndPhotosNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public InitializeAvitoCatalogueItemPhoneAndPhotosNode(
        AvitoParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        IAvitoChainNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Intializing avito catalogue item photos and phone");
        if (_pipeLine.CatalogueItems == null)
        {
            _logger.Log("Catalogue items were null. Stopping process.");
            return;
        }
        foreach (var catalogueItem in _pipeLine.CatalogueItems)
        {
            await new NavigateOnPageCommand(catalogueItem.UrlInfo).ExecuteAsync(_pipeLine.Provider);
            _logger.Log("Navigated on page. Starting fetching gallery");
            await new FetchMobilePhone(catalogueItem).ExecuteAsync(_pipeLine.Provider);
            _logger.Log("Mobile phone fetched.");
            if (string.IsNullOrWhiteSpace(catalogueItem.PublisherInfo))
            {
                _logger.Log("No publisher was provided or is agent. Skipping.");
                continue;
            }
            await new NavigateOnPageCommand(catalogueItem.UrlInfo).ExecuteAsync(_pipeLine.Provider);
            _logger.Log("Navigated on page. Starting fetching gallery");
            await new FetchGalleryItemsCommand(catalogueItem).ExecuteAsync(_pipeLine.Provider);
            _logger.Log("Gallery fetched. Starting fetching mobile phone");
        }
        _pipeLine.Provider.Dispose();
        _logger.Log("Web driver instance was disposed.");
        if (Next != null)
        {
            _logger.Log("Processing next step in chain");
            await Next.ExecuteAsync();
        }
    }
}
