using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Persistance.SDK;

public record GetAdvertisementsCountByServiceQuery(string ServiceName);

public static class GetAdvertisementsCountByServiceQueryExtensions
{
    public static IQueryable<Advertisement> CountByServiceQuery(
        this IQueryable<Advertisement> query,
        GetAdvertisementsCountByServiceQuery parameter
    )
    {
        query = query.Where(ad => ad.ServiceName.ServiceName == parameter.ServiceName);
        return query;
    }
}
