using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.OK.Parser.Filters;
using AdvSearcher.OK.Parser.Models.External;
using AdvSearcher.OK.Parser.Models.Internal;
using AdvSearcher.OK.Parser.Utils.Converters;
using AdvSearcher.OK.Parser.Utils.Factory.Builders;
using AdvSearcher.OK.Parser.Utils.Materials;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.HttpParsing;
using HtmlAgilityPack;

namespace AdvSearcher.OK.Parser.OkParserChains;

public sealed class OkParserPipeLine
{
    private readonly List<IParserResponse> _responses = [];
    private readonly IOkAdvertisementBuildersProvider _builders;
    private readonly OkDateConverter _converter;

    private OkTopicNodes? _nodes;
    private ServiceUrl? _url;

    public IReadOnlyCollection<IParserResponse> Responses => _responses;
    public OkTopicNodes? Nodes => _nodes;
    public ServiceUrl? Url => _url;
    public Action<int>? MaxProgressPublisher { get; set; }
    public Action<int>? CurrentProgressPublisher { get; set; }
    public Action<string>? NotificationsPublisher { get; set; }

    public int NodesCount
    {
        get
        {
            if (_nodes == null)
                return 0;
            return _nodes.Nodes.Count;
        }
    }

    public List<ParserFilterOption> Options { get; set; } = [];

    private IOkAdvertisementBuilder<DateOnly>? _dateBuilder;
    private IOkAdvertisementBuilder<string>? _urlBuilder;
    private IOkAdvertisementBuilder<string>? _idBuilder;
    private IOkAdvertisementBuilder<string>? _contentBuilder;
    private IOkAdvertisementBuilder<List<string>>? _attachmentsBuilder;
    private IOkAdvertisementBuilder<string>? _publisherBuilder;

    public OkParserPipeLine(IOkAdvertisementBuildersProvider builders, OkDateConverter converter)
    {
        _builders = builders;
        _converter = converter;
    }

    public void SetNodes(OkPageHtml html) => _nodes = new OkTopicNodes(html.PageHtml);

    public void SetServiceUrl(ServiceUrl url) => _url = url;

    public void InstantiateDateBuilder(HtmlNode node) =>
        _dateBuilder = _builders.GetDateOnlyBuilder(node, _converter);

    public async Task<Result<DateOnly>> GetAdvertisementDate()
    {
        if (_dateBuilder == null)
            return new Error("Date builder was not instantiated");
        return await _dateBuilder.Build();
    }

    public void InstantiatePublisherBuilder(HtmlNode node) =>
        _publisherBuilder = _builders.GetPublisherBuilder(node);

    public void InstantiateUrlBuilder(HtmlNode node)
    {
        if (_url == null)
            return;
        _urlBuilder = _builders.GetUrlBuilder(node, _url);
        _idBuilder = _builders.GetIdBuilder(_urlBuilder);
    }

    public void InstantiateContentBuilder(HtmlNode node, IHttpClient client)
    {
        if (_urlBuilder == null)
            return;
        HtmlDocument doc = new HtmlDocument();
        _contentBuilder = _builders.GetContentBuilder(_urlBuilder, client, doc);
        _attachmentsBuilder = _builders.GetAttachmentsBuilder(doc);
    }

    public async Task AddAdvertisementResponse(ParserFilter filter)
    {
        Result<IParsedAdvertisement> advertisement = await CreateParsedAdvertisement();
        Result<IParsedPublisher> publisher = await CreateParsedPublisher();
        IParsedAttachment[] attachments = await CreateParsedAttachments();
        if (advertisement.IsFailure)
            return;
        if (publisher.IsFailure)
            return;
        IParserFilterVisitor visitor = new OkAdvertisementsFilterVisitor(
            publisher.Value.Info,
            advertisement.Value.Id
        );
        if (!filter.IsMatchingFilters(visitor))
            return;
        _responses.Add(new OkParserResults(advertisement.Value, attachments, publisher.Value));
    }

    private async Task<Result<IParsedAdvertisement>> CreateParsedAdvertisement()
    {
        if (_idBuilder == null)
            return new Error("Id builder was null");
        if (_urlBuilder == null)
            return new Error("Url builder was null");
        if (_contentBuilder == null)
            return new Error("Content builder was null");
        if (_dateBuilder == null)
            return new Error("Date builder was null");

        Result<string> id = await _idBuilder.Build();
        Result<string> url = await _urlBuilder.Build();
        Result<string> content = await _contentBuilder.Build();
        Result<DateOnly> date = await _dateBuilder.Build();
        return OkParsedAdvertisement.Create(id, url, content, date);
    }

    private async Task<Result<IParsedPublisher>> CreateParsedPublisher()
    {
        if (_publisherBuilder == null)
            return new Error("Publisher builder was null");
        Result<string> publisherInfo = await _publisherBuilder.Build();
        return OkParsedPublisher.Create(publisherInfo);
    }

    private async Task<IParsedAttachment[]> CreateParsedAttachments()
    {
        if (_attachmentsBuilder == null)
            return [];
        Result<List<string>> attachments = await _attachmentsBuilder.Build();
        if (attachments.IsFailure)
            return [];
        List<IParsedAttachment> parsedAttachments = new List<IParsedAttachment>();
        foreach (var attachment in attachments.Value)
        {
            Result<IParsedAttachment> result = OkParsedAttachment.Create(attachment);
            if (result.IsFailure)
                continue;
            parsedAttachments.Add(result.Value);
        }
        return parsedAttachments.ToArray();
    }
}
