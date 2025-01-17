using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Tools;
using LiteDB.Async;

namespace AdvSearcher.Persistance.Cache;

public sealed class CachedAdvertisements : ICachedAdvertisementsRepository
{
    private readonly LiteDatabase _db = new LiteDatabase();

    public async Task<Result<CachedAdvertisementOperationType>> Add(
        CachedAdvertisement advertisement
    )
    {
        using LiteDatabaseAsync db = _db.GetDb();
        var collection = db.GetCollection<CachedAdvertisement>(LiteDatabase.CollectionName);
        if (await IsItemAlreadyExists(collection, advertisement))
            return new Error("Advertisement already cached");
        await collection.EnsureIndexAsync(x => x.Key, unique: true);
        await collection.InsertAsync(advertisement);
        return CachedAdvertisementOperationType.Success;
    }

    public async Task<Result<CachedAdvertisementOperationType>> Clear()
    {
        using LiteDatabaseAsync db = _db.GetDb();
        var collection = db.GetCollection<CachedAdvertisement>(LiteDatabase.CollectionName);
        await collection.DeleteAllAsync();
        return CachedAdvertisementOperationType.Success;
    }

    public async Task<IEnumerable<CachedAdvertisement>> GetAll()
    {
        using LiteDatabaseAsync db = _db.GetDb();
        var collection = db.GetCollection<CachedAdvertisement>(LiteDatabase.CollectionName);
        var data = await collection.FindAllAsync();
        return data;
    }

    public async Task<int> GetCacheCount()
    {
        using LiteDatabaseAsync db = _db.GetDb();
        var collection = db.GetCollection<CachedAdvertisement>(LiteDatabase.CollectionName);
        int count = await collection.CountAsync();
        return count;
    }

    private async Task<bool> IsItemAlreadyExists(
        ILiteCollectionAsync<CachedAdvertisement> collection,
        CachedAdvertisement advertisement
    ) => await collection.ExistsAsync(ad => ad.Key == advertisement.Key);
}
