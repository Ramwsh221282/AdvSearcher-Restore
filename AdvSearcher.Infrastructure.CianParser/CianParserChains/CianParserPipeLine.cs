using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastructure.CianParser.CianParserChains.PipeLineComponents;
using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;
using AdvSearcher.Infrastructure.CianParser.Utils.Converters;
using AdvSearcher.Infrastructure.CianParser.Utils.Factories;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains;

internal sealed class CianParserPipeLine
{
    private readonly CianDateConverter _converter;
    private readonly List<IParserResponse> _responses = [];
    public WebDriverProvider Provider { get; }
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    public CianParserPipeLine(CianDateConverter converter, WebDriverProvider provider)
    {
        _converter = converter;
        Provider = provider;
    }

    private ServiceUrl? _url;
    public ServiceUrl? Url => _url;

    private CianCardsWrapperElement? _wrapperElement;
    public CianCardsWrapperElement? WrapperElement => _wrapperElement;

    private CianCardElement[]? _cardElements;
    public CianCardElement[]? CardElements => _cardElements;

    private CianAdvertisementCard[]? _advertisementCards;
    public CianAdvertisementCard[]? AdvertisementCards => _advertisementCards;

    private CianAdvertisementsFactory? _factory;

    public void SetServiceUrl(ServiceUrl url)
    {
        if (_url != null)
            return;
        _url = url;
    }

    public void SetCardsWrapperElement(CianCardsWrapperElement element)
    {
        if (_wrapperElement != null)
            return;
        _wrapperElement = element;
    }

    public void SetCardElements(CianCardElement[] elements)
    {
        if (_cardElements != null)
            return;
        _cardElements = elements;
    }

    public void SetCianAdvertisementCards(CianAdvertisementCard[] cards)
    {
        if (_advertisementCards != null)
            return;
        _advertisementCards = cards;
    }

    public void InstantiateFactory()
    {
        if (_factory != null)
            return;
        if (_advertisementCards == null)
            return;
        _factory = new CianAdvertisementsFactory(_advertisementCards, _converter);
    }

    public void ConstructResponses()
    {
        if (_factory == null)
            return;
        _responses.AddRange(_factory.Construct());
    }
}
