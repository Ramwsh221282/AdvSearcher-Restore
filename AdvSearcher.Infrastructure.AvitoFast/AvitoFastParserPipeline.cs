using System.Collections;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep.Models;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast;

internal sealed class AvitoFastParserPipeline : IEnumerable<AvitoAdvertisement>
{
    public ServiceUrl? Url { get; private set; }
    private List<AvitoAdvertisement> _advertisements = [];
    public List<IParserResponse> Results { get; init; } = [];

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

    public void CleanFromAgents()
    {
        _advertisements = _advertisements.Where(ad => ad.IsAgent == false).ToList();
    }

    public IEnumerator<AvitoAdvertisement> GetEnumerator()
    {
        foreach (AvitoAdvertisement advertisement in _advertisements)
            yield return advertisement;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
