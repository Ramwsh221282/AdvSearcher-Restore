using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal sealed class OkPublisherBuilder(HtmlNode node) : IOkAdvertisementBuilder<string>
{
    private const string Path = "//div[@class='feed_h']";

    public async Task<Result<string>> Build()
    {
        var selectedNode = node.SelectSingleNode(Path);
        return selectedNode == null
            ? await Task.FromResult(ParserErrors.CantParsePublisher)
            : await Task.FromResult(selectedNode.InnerText);
    }
}
