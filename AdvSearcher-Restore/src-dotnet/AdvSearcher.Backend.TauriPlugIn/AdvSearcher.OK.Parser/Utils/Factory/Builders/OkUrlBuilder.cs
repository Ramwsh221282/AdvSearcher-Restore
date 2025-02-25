using System.Text;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using HtmlAgilityPack;

namespace AdvSearcher.OK.Parser.Utils.Factory.Builders;

public sealed class OkUrlBuilder(HtmlNode node, ServiceUrl url) : IOkAdvertisementBuilder<string>
{
    private const string Path = "//div[@class='media-text_cnt_tx emoji-tx textWrap']";
    private const string AttributeName = "data-tid";

    public async Task<Result<string>> Build()
    {
        var selectedNode = node.SelectSingleNode(Path);
        var urlAttribute = selectedNode.GetAttributeValue(AttributeName, string.Empty);
        if (string.IsNullOrWhiteSpace(urlAttribute))
            return ParserErrors.CantParseUrl;

        var stringBuilder = new StringBuilder(url.Value.Value)
            .Replace("topics", "topic")
            .Append('/')
            .Append(urlAttribute);
        return await Task.FromResult<Result<string>>(stringBuilder.ToString());
    }
}
