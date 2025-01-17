using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.VK.Parser.Filtering;

public sealed class VkParserDateOnlyFilterVisitor : IParserFilterVisitor
{
    private readonly IParsedAdvertisement _advertisement;

    public VkParserDateOnlyFilterVisitor(IParsedAdvertisement advertisement) =>
        _advertisement = advertisement;

    public bool Visit(ParserFilterWithDate filter) =>
        _advertisement.Date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
