using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Core.Entities.Publishers;

namespace AdvSearcher.Parser.SDK.Filtering;

// union for filter types.
public abstract record ParserFilterOption;

// filter by date.
public sealed record ParserFilterWithDate(DateOnly StartDate, DateOnly EndDate)
    : ParserFilterOption;

// filter by ignore names.
public sealed record ParserFilterWithIgnoreNames(IEnumerable<Publisher> IgnoredPublishers)
    : ParserFilterOption;

// filter by cache. Cache - deleted ads that added in cache. So no more need to look for removed advertisement.
public sealed record ParserFilterWithCachedAdvertisements(
    IEnumerable<CachedAdvertisement> CachedAdvertisements
) : ParserFilterOption;

public static class ParserFilterExtensions
{
    public static List<ParserFilterOption> CreateOptionsList(
        DateOnly startDate = default,
        DateOnly endDate = default,
        IEnumerable<Publisher>? ignoredPublishers = null,
        IEnumerable<CachedAdvertisement>? cached = null
    )
    {
        List<ParserFilterOption> options = [];
        if (startDate != default && endDate != default)
            options.Add(new ParserFilterWithDate(startDate, endDate));
        if (ignoredPublishers != null)
            options.Add(new ParserFilterWithIgnoreNames(ignoredPublishers));
        if (cached != null)
            options.Add(new ParserFilterWithCachedAdvertisements(cached));
        return options;
    }

    public static bool BelongsPeriod(this DateOnly date, DateOnly startDate, DateOnly endDate)
    {
        if (startDate == default || endDate == default)
            return false;
        return date >= startDate && date <= endDate;
    }
}

public sealed class ParserFilter
{
    private readonly List<ParserFilterOption> _options;

    public ParserFilter(List<ParserFilterOption> options) => _options = options;

    public bool IsMatchingFilters(IParserFilterVisitor visitor)
    {
        foreach (ParserFilterOption option in _options)
        {
            bool result = option switch
            {
                ParserFilterWithDate date => visitor.Visit(date),
                ParserFilterWithIgnoreNames ignoreNames => visitor.Visit(ignoreNames),
                ParserFilterWithCachedAdvertisements cached => visitor.Visit(cached),
                _ => true,
            };
            if (!result)
                return false;
        }
        return true;
    }
}

public interface IParserFilterVisitor
{
    bool Visit(ParserFilterWithDate filter);
    bool Visit(ParserFilterWithIgnoreNames filter);
    bool Visit(ParserFilterWithCachedAdvertisements filter);
}
