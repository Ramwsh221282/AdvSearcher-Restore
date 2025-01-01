using LiteDB.Async;

namespace AdvSearcher.Persistance.AdvertisementsCache;

internal sealed class LiteDatabase
{
    public const string ConnectionString = @"Filename=CachedAdvertisements.db; Connection=Shared";
    public const string CollectionName = "Cached";

    public LiteDatabaseAsync GetDb() => new LiteDatabaseAsync(ConnectionString);
}
