using System.Collections;
using AdvSearcher.Avito.Parser.Filtering;
using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Avito.Parser.Steps.FourthStep.Converters;
using AdvSearcher.Avito.Parser.Steps.SecondStep.Models;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Avito.Parser;

public sealed class AvitoFastParserPipeline : IEnumerable<AvitoAdvertisement>
{
    public ServiceUrl? Url { get; private set; }
    private List<AvitoAdvertisement> _advertisements = [];
    public List<IParserResponse> Results { get; private set; } = [];
    public Action<int>? MaxProgressPublisher { get; set; }
    public Action<int>? CurrentProgressPublisher { get; set; }
    public Action<string>? NotificationsPublisher { get; set; }

    public List<ParserFilterOption> Options { get; set; } = [];

    public int Total => _advertisements.Count;

    public void SetServiceUrl(ServiceUrl url)
    {
        if (Url != null)
            return;
        Url = url;
    }

    public void AddAdvertisement(CatalogueItemJson json)
    {
        AvitoAdvertisement advertisement = new AvitoAdvertisement(json);
        _advertisements.Add(advertisement);
    }

    public void CleanFromAgents() =>
        _advertisements = _advertisements.Where(ad => ad.IsAgent == false).ToList();

    public void FilterByDate()
    {
        List<AvitoAdvertisement> filtered = [];
        AvitoDateConverter converter = new AvitoDateConverter();
        ParserFilter filter = new ParserFilter(Options);
        foreach (var advertisement in _advertisements)
        {
            Result<DateOnly> date = converter.Convert(advertisement.Date);
            if (date.IsFailure)
                continue;
            IParserFilterVisitor visitor = new AvitoFastDateOnlyFiltering(date.Value);
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(advertisement);
        }
        _advertisements = filtered;
    }

    public void FilterByCache()
    {
        List<AvitoAdvertisement> filtered = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var advertisement in _advertisements)
        {
            if (string.IsNullOrWhiteSpace(advertisement.Id))
                continue;
            IParserFilterVisitor visitor = new AvitoFastCacheOnlyFiltering(advertisement.Id);
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(advertisement);
        }
        _advertisements = filtered;
    }

    public void FilterByPublishers()
    {
        if (!Results.Any())
            return;
        List<IParserResponse> filtered = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var result in Results)
        {
            IParserFilterVisitor visitor = new AvitoFastPublisherOnlyFiltering(
                result.Publisher.Info
            );
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(result);
        }
        Results = filtered;
    }

    public IEnumerator<AvitoAdvertisement> GetEnumerator()
    {
        foreach (AvitoAdvertisement advertisement in _advertisements)
            yield return advertisement;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
