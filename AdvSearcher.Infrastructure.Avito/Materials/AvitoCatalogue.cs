using HtmlAgilityPack;

namespace AdvSearcher.Infrastructure.Avito.Materials;

internal sealed class AvitoCatalogue
{
    public HtmlDocument Document { get; init; }

    public AvitoCatalogue(string html)
    {
        Document = new HtmlDocument();
        Document.LoadHtml(html);
    }
}
