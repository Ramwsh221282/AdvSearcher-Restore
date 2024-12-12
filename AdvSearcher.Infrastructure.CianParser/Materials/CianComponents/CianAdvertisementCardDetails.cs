namespace AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;

internal sealed class CianAdvertisementCardDetails
{
    public string Id { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DateDescription { get; set; } = string.Empty;
    public List<string> PhotoUrls { get; set; } = [];
}
