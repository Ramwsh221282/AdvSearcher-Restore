using System.Text;
using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastructure.Avito.Materials;

internal sealed class AvitoCatalogueItem
{
    public string Id { get; private set; }
    public string Description { get; private set; }
    public string Price { get; private set; }
    public string GeoInfo { get; private set; }
    public string DateInfo { get; private set; }
    public string UrlInfo { get; private set; }

    public List<string> PhotoUrls { get; init; } = [];
    public string PublisherInfo { get; set; } = string.Empty;
    public bool IsHomeOwner { get; set; } = false;
    public bool IsCorrect { get; private set; }

    private AvitoCatalogueItem(HtmlNode node)
    {
        IsCorrect = true;
        Id = ExtractId(node);
        var innerDocument = CreateInnerDocument(node);
        Description = ExtractDescription(innerDocument);
        Price = ExtractPrice(innerDocument);
        GeoInfo = ExtractGeoInfo(innerDocument);
        DateInfo = ExtractDate(innerDocument);
        UrlInfo = ExtractUrl(innerDocument);
    }

    private static HtmlDocument CreateInnerDocument(HtmlNode node)
    {
        var innerDocument = new HtmlDocument();
        innerDocument.LoadHtml(node.InnerHtml);
        return innerDocument;
    }

    private string ExtractId(HtmlNode node)
    {
        const string attributeName = "data-item-id";
        var attribute = node.Attributes[attributeName].Value;
        if (!string.IsNullOrWhiteSpace(attribute))
            return attribute;
        IsCorrect = false;
        return string.Empty;
    }

    private string ExtractDescription(HtmlDocument innerDocument)
    {
        const string path = "//*[@itemprop='description']";
        const string attribute = "content";
        var descriptionNode = innerDocument.DocumentNode.SelectSingleNode(path);
        if (descriptionNode == null)
        {
            IsCorrect = false;
            return string.Empty;
        }

        var content = descriptionNode.Attributes[attribute].Value;
        if (!string.IsNullOrWhiteSpace(content))
            return content;
        IsCorrect = false;
        return string.Empty;
    }

    private string ExtractPrice(HtmlDocument innerDocument)
    {
        const string path = "//*[@itemprop='price']";
        const string attribute = "content";
        var priceNode = innerDocument.DocumentNode.SelectSingleNode(path);
        if (priceNode == null)
        {
            IsCorrect = false;
            return string.Empty;
        }

        var price = priceNode.Attributes[attribute].Value;
        if (!string.IsNullOrWhiteSpace(price))
            return price;
        IsCorrect = false;
        return string.Empty;
    }

    private string ExtractGeoInfo(HtmlDocument innerDocument)
    {
        const string path = "//*[@data-marker='item-address']";
        var node = innerDocument.DocumentNode.SelectSingleNode(path);
        if (node != null)
            return node.InnerText;
        IsCorrect = false;
        return string.Empty;
    }

    private string ExtractDate(HtmlDocument document)
    {
        const string path = "//*[@data-marker='item-date']";
        var node = document.DocumentNode.SelectSingleNode(path);
        if (node != null)
            return node.InnerText;
        IsCorrect = false;
        return string.Empty;
    }

    private string ExtractUrl(HtmlDocument document)
    {
        const string path = "//*[@itemprop='url']";
        const string mainUrl = "https://www.avito.ru";
        const string attribute = "href";
        var node = document.DocumentNode.SelectSingleNode(path);
        if (node == null)
        {
            IsCorrect = false;
            return string.Empty;
        }
        var href = node.Attributes[attribute].Value;
        if (!string.IsNullOrWhiteSpace(href))
            return new StringBuilder(mainUrl).Append(href).ToString();
        IsCorrect = false;
        return string.Empty;
    }

    public static Result<AvitoCatalogueItem[]> CreateFromCatalogue(AvitoCatalogue catalogue)
    {
        const string itemsNode = "//*[@class='items-items-pZX46']";
        const string item = "//*[@data-marker='item']";
        var items = catalogue.Document.DocumentNode.SelectSingleNode(itemsNode);
        var data = items?.SelectNodes(item);
        if (data == null)
            return ParserErrors.HtmlEmpty;
        var extracted = new AvitoCatalogueItem[data.Count];
        for (var index = 0; index < data.Count; index++)
        {
            extracted[index] = new AvitoCatalogueItem(data[index]);
        }
        return extracted;
    }
}
