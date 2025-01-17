namespace AdvSearcher.Avito.Parser.Steps.SecondStep.Models;

public sealed record CatalogueItemJson
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

public sealed record SellerInfo(string Value = "");

public sealed record PhotoInfo(string Value = "");

public sealed record SourceUrl(string Value = "");

public sealed record AdvertisementId(string Value = "");

public sealed record Address(string Value = "");

public sealed record Price(string Value = "");

public sealed record Title(string Value = "");

public sealed record PublishedDate(string Value = "");
