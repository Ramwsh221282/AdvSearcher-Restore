using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain;

internal sealed class AvitoParserPipeLine
{
    private readonly AvitoDateConverter _converter;
    private readonly WebDriverProvider _provider;

    public WebDriverProvider Provider => _provider;
    public List<IParserResponse> Responses { get; private set; } = [];

    private AvitoCatalogueItem[]? _catalogueItems;
    public AvitoCatalogueItem[]? CatalogueItems => _catalogueItems;

    private ServiceUrl? _url;
    public ServiceUrl? Url => _url;

    public AvitoParserPipeLine(AvitoDateConverter converter, WebDriverProvider provider)
    {
        _converter = converter;
        _provider = provider;
    }

    public void SetServiceUrl(ServiceUrl url)
    {
        if (_url != null)
            return;
        _url = url;
    }

    public void SetCatalogueItems(AvitoCatalogue catalogue)
    {
        if (_catalogueItems != null)
            return;
        Result<AvitoCatalogueItem[]> items = AvitoCatalogueItem.CreateFromCatalogue(catalogue);
        if (items.IsFailure)
            return;
        _catalogueItems = items.Value.Where(i => i.IsCorrect).ToArray();
    }

    public void ConstructParserResponse()
    {
        if (_catalogueItems == null)
            return;
        AvitoAdvertisementsFactory factory = new AvitoAdvertisementsFactory();
        Responses = factory.Construct(_catalogueItems, _converter);
    }
}
