using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class InitializeAvitoCatalogueItemPhoneAndPhotosNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public InitializeAvitoCatalogueItemPhoneAndPhotosNode(
        AvitoParserPipeLine pipeLine,
        IAvitoChainNode? next = null
    )
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.CatalogueItems == null)
            return;
        foreach (var catalogueItem in _pipeLine.CatalogueItems)
        {
            await new NavigateOnPageCommand(catalogueItem.UrlInfo).ExecuteAsync(_pipeLine.Provider);
            await new FetchGalleryItemsCommand(catalogueItem).ExecuteAsync(_pipeLine.Provider);
            await new FetchMobilePhone(catalogueItem).ExecuteAsync(_pipeLine.Provider);
        }
        _pipeLine.Provider.Dispose();
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
