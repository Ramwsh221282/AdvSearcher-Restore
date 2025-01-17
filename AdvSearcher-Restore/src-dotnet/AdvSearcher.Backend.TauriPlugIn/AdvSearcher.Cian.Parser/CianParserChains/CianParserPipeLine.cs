using AdvSearcher.Cian.Parser.CianParserChains.PipeLineComponents;
using AdvSearcher.Cian.Parser.Filtering;
using AdvSearcher.Cian.Parser.Materials.CianComponents;
using AdvSearcher.Cian.Parser.Utils.Converters;
using AdvSearcher.Cian.Parser.Utils.Factories;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Cian.Parser.CianParserChains;

public sealed class CianParserPipeLine
{
    private readonly CianDateConverter _converter;
    private List<IParserResponse> _responses = [];
    public WebDriverProvider Provider { get; }
    public IReadOnlyCollection<IParserResponse> Responses => _responses;
    public List<ParserFilterOption> Options { get; set; } = [];
    public Action<int>? MaxProgressPublisher { get; set; }
    public Action<int>? CurrentProgressPublisher { get; set; }
    public Action<string>? NotificationsPublisher { get; set; }

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
        MaxProgressPublisher?.Invoke(_responses.Count);
        CurrentProgressPublisher?.Invoke(_responses.Count);
    }

    public void ProcessFiltering()
    {
        if (!_responses.Any())
            return;
        ParserFilter filter = new ParserFilter(Options);
        List<IParserResponse> filtered = [];
        foreach (var response in _responses)
        {
            IParserFilterVisitor visitor = new CianFiltering(
                response.Advertisement,
                response.Publisher
            );
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(response);
        }
        _responses = filtered;
    }
}
