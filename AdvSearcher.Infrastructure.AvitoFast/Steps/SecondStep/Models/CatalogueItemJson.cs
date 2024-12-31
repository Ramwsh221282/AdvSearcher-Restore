namespace AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep.Models;

internal sealed record CatalogueItemJson
{
    public SellerInfo Seller { get; }
    public List<PhotoInfo> Photos { get; }
    public Price Price { get; }
    public SourceUrl SourceUrl { get; }
    public Title Title { get; }
    public Address Address { get; }
    public PublishedDate PublishedDate { get; }
    public AdvertisementId AdvertisementId { get; }

    public CatalogueItemJson(
        SellerInfo info,
        List<PhotoInfo> photos,
        Price price,
        Title title,
        SourceUrl sourceUrl,
        Address address,
        PublishedDate publishedDate,
        AdvertisementId id
    )
    {
        Seller = info;
        Photos = photos;
        Price = price;
        Title = title;
        SourceUrl = sourceUrl;
        Address = address;
        PublishedDate = publishedDate;
        AdvertisementId = id;
    }
}

internal sealed record SellerInfo(string Value = "");

internal sealed record PhotoInfo(string Value = "");

internal sealed record SourceUrl(string Value = "");

internal sealed record AdvertisementId(string Value = "");

internal sealed record Address(string Value = "");

internal sealed record Price(string Value = "");

internal sealed record Title(string Value = "");

internal sealed record PublishedDate(string Value = "");
