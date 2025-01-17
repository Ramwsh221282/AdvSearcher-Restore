using System.Diagnostics;
using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public sealed record ParsedDataResponse(
    ulong Id,
    string SourceUrl,
    string Content,
    string Publisher,
    List<string> ImageLinks,
    string PublishDate,
    string ParsedDate
);

public sealed record PaginationRequest(int PageSize);

public sealed record AdvertisementRequest(ulong AdvertisementId);

public sealed class ParsedDataController(
    PersistanceServiceFactory factory,
    IEventPublisher publisher
)
{
    private readonly PersistanceServiceFactory _factory = factory;
    private readonly IEventPublisher _publisher = publisher;
    private const string Listener = "data-listener";
    private int _count;

    public IEnumerable<ParsedDataResponse> GetParsedData(GetAdvertisementsQuery query)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        IEnumerable<Advertisement> advertisements = repository.GetAll(query).Result;
        return advertisements.ToResponse();
    }

    public int GetCountQuery(GetAdvertisementsCountByServiceQuery query)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        _count = repository.GetCount(query).Result;
        return _count;
    }

    public int[] GetPagesQuery(PaginationRequest request)
    {
        int totalPages = (int)Math.Ceiling((double)_count / request.PageSize);
        int[] pages = new int[totalPages];
        for (int index = 0; index < totalPages; index++)
            pages[index] = index + 1;
        return pages;
    }

    public void RemoveAdvertisement(AdvertisementRequest request)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        AdvertisementId id = AdvertisementId.Create(request.AdvertisementId);
        Result<Advertisement> advertisement = repository.GetById(id).Result;
        if (advertisement.IsFailure)
        {
            _publisher.Publish(Listener, advertisement.Error.Description);
            return;
        }
        repository.Delete(advertisement).Wait();
        _publisher.Publish(Listener, $"Запись ID: {request.AdvertisementId} удалена");
    }

    public void RemoveAndCacheAdvertisement(AdvertisementRequest request)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        ICachedAdvertisementsRepository cache = _factory.CreateCachedAdvertisementsRepository(
            context
        );
        AdvertisementId id = AdvertisementId.Create(request.AdvertisementId);
        Result<Advertisement> advertisement = repository.GetById(id).Result;
        if (advertisement.IsFailure)
        {
            _publisher.Publish(Listener, advertisement.Error.Description);
            return;
        }
        CachedAdvertisement cached = advertisement.Value.ToCachedAdvertisement();
        Result<CachedAdvertisementOperationType> caching = cache.Add(cached).Result;
        if (caching.IsFailure)
        {
            _publisher.Publish(Listener, caching.Error.Description);
            return;
        }
        repository.Delete(advertisement).Wait();
        _publisher.Publish(Listener, $"Запись ID: {request.AdvertisementId} кеширована");
    }

    public void RemoveAllAdvertisements(GetAdvertisementsByServiceNameOnlyQuery query)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        Advertisement[] advertisements = repository.GetAll(query).Result.ToArray();
        for (int index = 0; index < advertisements.Length; index++)
            repository.Delete(advertisements[index]).Wait();
        _publisher.Publish(Listener, $"Записи {query.ServiceName} удалены");
    }

    public void RemoveAndCacheAllAdvertisements(GetAdvertisementsByServiceNameOnlyQuery query)
    {
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        ICachedAdvertisementsRepository cache = _factory.CreateCachedAdvertisementsRepository(
            context
        );
        Advertisement[] advertisements = repository.GetAll(query).Result.ToArray();
        CachedAdvertisement[] cached = advertisements
            .Select(ad => ad.ToCachedAdvertisement())
            .ToArray();
        for (int index = 0; index < advertisements.Length; index++)
        {
            repository.Delete(advertisements[index]).Wait();
            cache.Add(cached[index]).Wait();
        }
        _publisher.Publish(Listener, $"Записи {query.ServiceName} кешированы");
    }

    public void OpenAdvertisementInBrowser(AdvertisementRequest request)
    {
        Result<AdvertisementId> id = AdvertisementId.Create(request.AdvertisementId);
        if (id.IsFailure)
            return;
        using PersistanceContext context = _factory.CreateContext();
        IAdvertisementsRepository repository = _factory.CreateAdvertisementsRepository(context);
        Result<Advertisement> advertisement = repository.GetById(id).Result;
        if (advertisement.IsFailure)
            return;
        Process.Start(
            new ProcessStartInfo { FileName = advertisement.Value.Url.Url, UseShellExecute = true }
        );
    }
}

public static class ParsedDataControllerExtensions
{
    public static List<ParsedDataResponse> ToResponse(
        this IEnumerable<Advertisement> advertisements
    )
    {
        List<ParsedDataResponse> response = [];
        foreach (var advertisement in advertisements)
        {
            ParsedDataResponse responseItem = new ParsedDataResponse(
                advertisement.Id.Id,
                advertisement.Url.Url,
                advertisement.Content.Content,
                advertisement.Publisher!.Data.Value,
                advertisement.Attachments.Select(at => at.Url.Value).ToList(),
                advertisement.Date.Value.ToString("dd.MM.yyyy"),
                advertisement.CreationDate.Date.ToShortDateString()
            );
            response.Add(responseItem);
        }
        return response;
    }
}
