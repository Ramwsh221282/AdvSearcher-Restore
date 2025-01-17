using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.OK.Parser.Utils.Converters;
using AdvSearcher.Parser.SDK.HttpParsing;
using HtmlAgilityPack;

namespace AdvSearcher.OK.Parser.Utils.Factory.Builders;

public sealed class OkAdvertisementBuilderProvider : IOkAdvertisementBuildersProvider
{
    public IOkAdvertisementBuilder<string> GetContentBuilder(
        IOkAdvertisementBuilder<string> builder,
        IHttpClient client,
        HtmlDocument document
    ) => new OkContentBuilder(builder, client, document);

    public IOkAdvertisementBuilder<string> GetUrlBuilder(HtmlNode node, ServiceUrl url) =>
        new OkUrlBuilder(node, url);

    public IOkAdvertisementBuilder<string> GetIdBuilder(IOkAdvertisementBuilder<string> builder) =>
        new OkIdBuilder(builder);

    public IOkAdvertisementBuilder<List<string>> GetAttachmentsBuilder(HtmlDocument document) =>
        new OkAttachmentsBuilder(document);

    public IOkAdvertisementBuilder<string> GetPublisherBuilder(HtmlNode node) =>
        new OkPublisherBuilder(node);

    public IOkAdvertisementBuilder<DateOnly> GetDateOnlyBuilder(
        HtmlNode node,
        OkDateConverter converter
    ) => new OkDateBuilder(node, converter);
}
