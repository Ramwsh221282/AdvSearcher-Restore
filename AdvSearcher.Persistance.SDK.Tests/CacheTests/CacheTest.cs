using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK.Tests.CacheTests;

[TestFixture]
[Category("Cache")]
public class CacheTest
{
    private readonly IServiceCollection _services;

    public CacheTest()
    {
        _services = new ServiceCollection();
        _services = _services.AddPersistanceSDK();
    }

    [Test, Order(1)]
    public async Task CleanCache()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        ICachedAdvertisementsRepository repository =
            provider.GetRequiredService<ICachedAdvertisementsRepository>();
        Result<CachedAdvertisementOperationType> result = await repository.Clear();
        bool isSuccess = result.IsSuccess;
        Assert.That(isSuccess, Is.True);
    }

    [Test, Order(2)]
    public async Task Add_In_Cache_Test()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        ICachedAdvertisementsRepository repository =
            provider.GetRequiredService<ICachedAdvertisementsRepository>();
        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName,
            url,
            createdDate,
            type
        );
        CachedAdvertisement cached = advertisement.ToCachedAdvertisement();
        Result<CachedAdvertisementOperationType> result = await repository.Add(cached);
        bool isSuccess = result.IsSuccess;
        await repository.Clear();
        Assert.That(isSuccess, Is.True);
    }

    [Test, Order(3)]
    public async Task Add_Duplicate_In_Cache_Test()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        ICachedAdvertisementsRepository repository =
            provider.GetRequiredService<ICachedAdvertisementsRepository>();
        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName,
            url,
            createdDate,
            type
        );
        CachedAdvertisement cached1 = advertisement.ToCachedAdvertisement();
        CachedAdvertisement cached2 = advertisement.ToCachedAdvertisement();
        Result<CachedAdvertisementOperationType> result1 = await repository.Add(cached1);
        Result<CachedAdvertisementOperationType> result2 = await repository.Add(cached2);
        bool isFailure = result2.IsFailure;
        await repository.Clear();
        Assert.That(isFailure, Is.True);
    }

    [Test, Order(4)]
    public async Task Add_WithSameId_DifferentServices_Test()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        ICachedAdvertisementsRepository repository =
            provider.GetRequiredService<ICachedAdvertisementsRepository>();
        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName1 = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement1 = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName1,
            url,
            createdDate,
            type
        );

        AdvertisementServiceName serviceName2 = AdvertisementServiceName.Create("Other service");
        Advertisement advertisement2 = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName2,
            url,
            createdDate,
            type
        );

        CachedAdvertisement cached1 = advertisement1.ToCachedAdvertisement();
        CachedAdvertisement cached2 = advertisement2.ToCachedAdvertisement();
        Result<CachedAdvertisementOperationType> result1 = await repository.Add(cached1);
        Result<CachedAdvertisementOperationType> result2 = await repository.Add(cached2);
        bool bothSuccess = result1.IsSuccess && result2.IsSuccess;
        await repository.Clear();
        Assert.That(bothSuccess, Is.True);
    }

    [Test, Order(5)]
    public async Task Test_Cached_Advertisements_Count()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        ICachedAdvertisementsRepository repository =
            provider.GetRequiredService<ICachedAdvertisementsRepository>();
        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName1 = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement1 = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName1,
            url,
            createdDate,
            type
        );

        AdvertisementServiceName serviceName2 = AdvertisementServiceName.Create("Other service");
        Advertisement advertisement2 = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName2,
            url,
            createdDate,
            type
        );

        CachedAdvertisement cached1 = advertisement1.ToCachedAdvertisement();
        CachedAdvertisement cached2 = advertisement2.ToCachedAdvertisement();
        Result<CachedAdvertisementOperationType> result1 = await repository.Add(cached1);
        Result<CachedAdvertisementOperationType> result2 = await repository.Add(cached2);

        int count = await repository.GetCacheCount();
        Assert.That(count, Is.EqualTo(2));
    }
}
