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

// filter option with all 3 above.
public sealed record ParserFilterOptionCompleted(List<ParserFilterOption> Options)
    : ParserFilterOption;

// filter option without any filtering mode.
public sealed record ParserFilterOptionEmpty : ParserFilterOption;

public static class ParserFilterOptionExtensions
{
    // static factory method to create parser filter option.
    public static ParserFilterOption Create(
        DateOnly startDate = default,
        DateOnly endDate = default,
        IEnumerable<Publisher>? Publishers = null,
        IEnumerable<CachedAdvertisement>? Cached = null
    )
    {
        List<ParserFilterOption> options = [];
        if (startDate != default && endDate != default)
            options.Add(new ParserFilterWithDate(startDate, endDate));
        if (Publishers != null)
            options.Add(new ParserFilterWithIgnoreNames(Publishers));
        if (Cached != null)
            options.Add(new ParserFilterWithCachedAdvertisements(Cached));
        return options.Any()
            ? new ParserFilterOptionCompleted(options)
            : new ParserFilterOptionEmpty();
    }

    // extension method to match filter option with visitor. Visitor is a contract that each service that needs to be used with filter should implement.
    public static bool IsMatchingFilter(
        this ParserFilterOption option,
        IParserFilterOptionVisitor visitor
    )
    {
        bool matching = option switch
        {
            ParserFilterWithDate date => visitor.MatchesFilterOption(date),
            ParserFilterWithIgnoreNames ignoreNames => visitor.MatchesFilterOption(ignoreNames),
            ParserFilterWithCachedAdvertisements cachedAdvertisements =>
                visitor.MatchesFilterOption(cachedAdvertisements),
            ParserFilterOptionEmpty => false,
            _ => false,
        };
        return matching;
    }
}
