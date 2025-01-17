using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Avito.Parser.Steps.SecondStep.Models;

public sealed class CatalogueItemJsonBuilder
{
    private const string Pattern = @"_(\d+)\?";
    private static readonly Regex RegexPattern = new Regex(Pattern, RegexOptions.Compiled);

    private readonly JToken _value;
    private readonly JArray _galleryItems;
    private readonly JArray _childrens;

    private SellerInfo _sellerInfo = new SellerInfo();
    private List<PhotoInfo> _photos = [];
    private Price _price = new Price();
    private SourceUrl _sourceUrl = new SourceUrl();
    private Title _title = new Title();
    private Address _address = new Address();
    private PublishedDate _publishedDate = new PublishedDate();
    private AdvertisementId _advertisementId = new AdvertisementId();

    public CatalogueItemJsonBuilder(JToken item)
    {
        _value = item["value"]!;
        _galleryItems = (JArray)_value["galleryItems"]!;
        JArray freeForm = (JArray)_value["freeForm"]!;
        foreach (var token in freeForm)
        {
            JToken? type = token["type"];
            if (type == null)
                continue;
            string typeName = type.ToString();
            if (typeName == "spreadContainer")
            {
                _childrens = (JArray)
                    token["content"]!["leftChildren"]![0]!["content"]!["children"]!;
                return;
            }
        }
        JToken content = freeForm[0]["content"]!;
        JArray leftChildren = (JArray)content["leftChildren"]!;
        _childrens = (JArray)leftChildren[0]["content"]!["children"]!;
    }

    public CatalogueItemJson Build()
    {
        InitializeSellerInfo();
        InitializePhotos();
        InitializePrice();
        InitializeTitle();
        InitializeUri();
        InitializeDateAndAddress();
        InitializeId();
        ReInitializeSourceUrl();
        return new CatalogueItemJson(
            _sellerInfo,
            _photos,
            _price,
            _title,
            _sourceUrl,
            _address,
            _publishedDate,
            _advertisementId
        );
    }

    private void InitializePrice()
    {
        JToken? price = _value["price"]?["current"];
        if (price == null)
            return;
        _price = new Price(price.ToString());
    }

    private void InitializeTitle()
    {
        JToken? title = _value["title"];
        if (title == null)
            return;
        _title = new Title(title.ToString());
    }

    private void InitializeUri()
    {
        JToken? uri = _value["uri"];
        if (uri == null)
            return;
        _sourceUrl = new SourceUrl(uri.ToString());
    }

    private void InitializePhotos()
    {
        foreach (JToken token in _galleryItems)
        {
            JToken? type = token["type"];
            if (type == null)
                continue;
            string typeName = type.ToString();
            if (typeName == "image")
            {
                JToken? attributes = token["value"];
                if (attributes != null)
                {
                    JToken lastToken = attributes.Last();
                    string lastValue = lastToken.ToString();
                    ReadOnlySpan<string> span = lastValue.Split(' ');
                    string imageUrl = span[^1].Replace("\"", "");
                    _photos.Add(new PhotoInfo(imageUrl));
                }
            }
        }
    }

    private void InitializeSellerInfo()
    {
        JToken? sellerInfo = _value["sellerInfo"];
        if (sellerInfo == null)
            return;
        JToken? name = sellerInfo["name"];
        if (name == null)
            return;
        _sellerInfo = new SellerInfo(name.ToString());
    }

    private void InitializeDateAndAddress()
    {
        foreach (JToken child in _childrens)
        {
            JToken? type = child["type"];
            if (type == null)
                continue;
            string typeName = type.ToString();
            if (typeName == "label")
            {
                JToken? labelContent = child["content"];
                if (labelContent == null)
                    return;
                JToken? labelId = labelContent["id"];
                if (labelId == null)
                    return;
                string labelString = labelId.ToString();
                if (labelString == "addressLabel")
                {
                    JToken? addressToken = labelContent["tokens"]![0]!["title"];
                    if (addressToken == null)
                        return;
                    _address = new Address(addressToken.ToString());
                }
                if (labelString == "dateLabelRich")
                {
                    JToken? dateToken = labelContent["tokens"]![0]!["title"];
                    if (dateToken == null)
                        return;
                    _publishedDate = new PublishedDate(dateToken.ToString());
                }
            }
        }
    }

    private void ReInitializeSourceUrl()
    {
        if (!string.IsNullOrWhiteSpace(_sourceUrl.Value))
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder = stringBuilder.Append("https://www.avito.ru");
            stringBuilder = stringBuilder.Append(_sourceUrl.Value);
            _sourceUrl = new SourceUrl(stringBuilder.ToString());
        }
    }

    private void InitializeId()
    {
        if (!string.IsNullOrWhiteSpace(_sourceUrl.Value))
        {
            string sourceUrl = _sourceUrl.Value;
            Match match = RegexPattern.Match(sourceUrl);
            if (match.Success)
            {
                string id = match.Groups[1].Value;
                _advertisementId = new AdvertisementId(id);
            }
        }
    }
}
