using System.Collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ConsoleForAvitoItemsJsonParse
{
    public class CatalogueItemsJsonContainer : IEnumerable<JToken>
    {
        private readonly JArray _items;

        private CatalogueItemsJsonContainer(JArray items) => _items = items;

        public static CatalogueItemsJsonContainer Create(JObject json)
        {
            JToken? result = json["result"];
            if (result == null)
                throw new NullReferenceException();
            JToken? items = result["items"];
            if (items == null)
                throw new NullReferenceException();
            return new CatalogueItemsJsonContainer((JArray)items);
        }

        public IEnumerator<JToken> GetEnumerator()
        {
            foreach (JToken item in _items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public CatalogueItemJson[] CreateCatalogueItemsArray()
        {
            List<CatalogueItemJson> items = [];
            foreach (var token in this)
            {
                JToken? type = token["type"];
                if (type == null)
                    continue;
                string typeName = type.ToString();
                if (typeName == "item")
                    items.Add(new CatalogueItemJsonBuilder(token).Build());
            }
            return items.ToArray();
        }
    }

    public class CatalogueItemJsonBuilder
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

    public record CatalogueItemJson
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

    public record SellerInfo(string Value = "");

    public record PhotoInfo(string Value = "");

    public record SourceUrl(string Value = "");

    public record AdvertisementId(string Value = "");

    public record Address(string Value = "");

    public record Price(string Value = "");

    public record Title(string Value = "");

    public record PublishedDate(string Value = "");

    class Program
    {
        static void Main()
        {
            string jsonData_0 = File.ReadAllText("avito_0.json");
            string jsonData_1 = File.ReadAllText("avito_1.json");
            string jsonData_2 = File.ReadAllText("avito_2.json");
            string jsonData_3 = File.ReadAllText("avito_3.json");
            string jsonData_4 = File.ReadAllText("avito_4.json");
            string jsonData_5 = File.ReadAllText("avito_5.json");
            string jsonData_6 = File.ReadAllText("avito_6.json");

            JObject json_0 = JObject.Parse(jsonData_0);
            JObject json_1 = JObject.Parse(jsonData_1);
            JObject json_2 = JObject.Parse(jsonData_2);
            JObject json_3 = JObject.Parse(jsonData_3);
            JObject json_4 = JObject.Parse(jsonData_4);
            JObject json_5 = JObject.Parse(jsonData_5);
            JObject json_6 = JObject.Parse(jsonData_6);

            CatalogueItemsJsonContainer container_0 = CatalogueItemsJsonContainer.Create(json_0);
            CatalogueItemsJsonContainer container_1 = CatalogueItemsJsonContainer.Create(json_1);
            CatalogueItemsJsonContainer container_2 = CatalogueItemsJsonContainer.Create(json_2);
            CatalogueItemsJsonContainer container_3 = CatalogueItemsJsonContainer.Create(json_3);
            CatalogueItemsJsonContainer container_4 = CatalogueItemsJsonContainer.Create(json_4);
            CatalogueItemsJsonContainer container_5 = CatalogueItemsJsonContainer.Create(json_5);
            CatalogueItemsJsonContainer container_6 = CatalogueItemsJsonContainer.Create(json_6);

            CatalogueItemJson[] items_0 = container_0.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_1 = container_1.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_2 = container_2.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_3 = container_3.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_4 = container_4.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_5 = container_5.CreateCatalogueItemsArray();
            CatalogueItemJson[] items_6 = container_6.CreateCatalogueItemsArray();

            int bpoint = 0;
        }
    }
}
