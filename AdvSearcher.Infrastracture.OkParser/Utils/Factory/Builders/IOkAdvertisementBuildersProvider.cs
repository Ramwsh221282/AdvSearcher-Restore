using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters;
using AdvSearcher.Parser.SDK.HttpParsing;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal interface IOkAdvertisementBuildersProvider
{
    public IOkAdvertisementBuilder<string> GetContentBuilder(
        IOkAdvertisementBuilder<string> builder,
        IHttpClient client,
        HtmlDocument document
    );

    public IOkAdvertisementBuilder<string> GetUrlBuilder(HtmlNode node, ServiceUrl url);
    public IOkAdvertisementBuilder<string> GetIdBuilder(IOkAdvertisementBuilder<string> builder);
    public IOkAdvertisementBuilder<List<string>> GetAttachmentsBuilder(HtmlDocument document);
    public IOkAdvertisementBuilder<string> GetPublisherBuilder(HtmlNode node);

    public IOkAdvertisementBuilder<DateOnly> GetDateOnlyBuilder(
        HtmlNode node,
        OkDateConverter converter
    );
}
