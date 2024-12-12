using AdvSearcher.Core.Tools;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal sealed class OkAttachmentsBuilder(HtmlDocument document)
    : IOkAdvertisementBuilder<List<string>>
{
    private const string Path = "//img[@class='media-photos_img']";
    private const string AttributeName = "src";

    private readonly List<string> _photoUrls = [];

    public async Task<Result<List<string>>> Build()
    {
        var nodes = document.DocumentNode.SelectNodes(Path);
        if (nodes == null)
            return _photoUrls;
        foreach (var node in nodes)
        {
            var src = node.GetAttributeValue(AttributeName, string.Empty);
            if (!string.IsNullOrWhiteSpace(src))
                _photoUrls.Add(src);
        }

        return await Task.FromResult(_photoUrls);
    }
}
