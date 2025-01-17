using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing;

public record ParsingOptions(
    string? StartDate,
    string? EndDate,
    bool WithIgnoreNames,
    bool WithCachedAds
);

public static class ParsingOptionsExtensions
{
    public static List<ParserFilterOption> CreateOptions(
        this ParsingOptions parsingOptions,
        PersistanceServiceFactory factory
    )
    {
        using PersistanceContext context = factory.CreateContext();
        DateOnly startDate = parsingOptions.StartDate.TryParseDate();
        DateOnly endDate = parsingOptions.EndDate.TryParseDate();
        CachedAdvertisement[]? cached = null;
        Publisher[]? publishers = null;
        if (parsingOptions.WithIgnoreNames)
        {
            IPublishersRepository repository = factory.CreatePublishersRepository(context);
            publishers = repository.GetAll().Result.ToArray();
        }
        if (parsingOptions.WithCachedAds)
        {
            ICachedAdvertisementsRepository cache = factory.CreateCachedAdvertisementsRepository(
                context
            );
            cached = cache.GetAll().Result.ToArray();
        }
        return ParserFilterExtensions.CreateOptionsList(startDate, endDate, publishers, cached);
    }

    private static DateOnly TryParseDate(this string? stringDate)
    {
        if (stringDate == null)
            return default;
        try
        {
            ReadOnlySpan<string> parts = stringDate.Split('.');
            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);
            return new DateOnly(year, month, day);
        }
        catch
        {
            return default;
        }
    }
}
