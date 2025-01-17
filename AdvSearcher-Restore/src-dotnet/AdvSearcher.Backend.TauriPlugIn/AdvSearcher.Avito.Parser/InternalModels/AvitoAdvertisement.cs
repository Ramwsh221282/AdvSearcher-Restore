using System.Text;
using AdvSearcher.Avito.Parser.Steps.SecondStep.Models;

namespace AdvSearcher.Avito.Parser.InternalModels;

public sealed class AvitoAdvertisement
{
    public string Id { get; set; } = string.Empty;
    public StringBuilder Description { get; set; } = new StringBuilder();
    public List<string> Photos { get; set; } = [];
    public StringBuilder PublisherInfo { get; set; } = new StringBuilder();
    public string Date { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public bool IsAgent { get; set; } = false;

    public AvitoAdvertisement() { }

    public AvitoAdvertisement(CatalogueItemJson json)
    {
        Id = json.AdvertisementId.Value;
        Description = Description
            .AppendLine(json.Title.Value)
            .AppendLine(json.Price.Value)
            .AppendLine(json.Address.Value);
        foreach (var photo in json.Photos)
        {
            Photos.Add(photo.Value);
        }
        PublisherInfo = PublisherInfo.AppendLine(json.Seller.Value);
        Date = json.PublishedDate.Value;
        SourceUrl = json.SourceUrl.Value;
    }
}
