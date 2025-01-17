using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Persistance.SDK;

public abstract record AdvertisementsDbQuery;

public record GetAdvertisementsQuery(string ServiceName, int page, int pageSize)
    : AdvertisementsDbQuery;

public record GetAdvertisementsByServiceNameOnlyQuery(string ServiceName) : AdvertisementsDbQuery;

public static class GetAdvertisementsQueryExtensions
{
    private static IQueryable<Advertisement> ApplyDefaultQuery(
        this IQueryable<Advertisement> queryable,
        GetAdvertisementsQuery query
    )
    {
        queryable = queryable.Where(ad => ad.ServiceName.ServiceName == query.ServiceName);
        queryable = queryable.Skip((query.page - 1) * query.pageSize);
        queryable = queryable.Take(query.pageSize);
        return queryable;
    }

    private static IQueryable<Advertisement> ApplyServiceOnlyQuery(
        this IQueryable<Advertisement> queryable,
        GetAdvertisementsByServiceNameOnlyQuery query
    )
    {
        queryable = queryable.Where(ad => ad.ServiceName.ServiceName == query.ServiceName);
        return queryable;
    }

    public static IQueryable<Advertisement> ApplyQueryParams(
        this IQueryable<Advertisement> queryable,
        AdvertisementsDbQuery query
    )
    {
        return query switch
        {
            GetAdvertisementsQuery original => queryable.ApplyDefaultQuery(original),
            GetAdvertisementsByServiceNameOnlyQuery serviceNameOnly =>
                queryable.ApplyServiceOnlyQuery(serviceNameOnly),
            _ => throw new NotSupportedException(),
        };
    }
}
