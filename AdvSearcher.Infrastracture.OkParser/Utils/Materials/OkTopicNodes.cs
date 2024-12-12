using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Materials;

internal sealed class OkTopicNodes
{
    private const string TopicPaths = "//div[contains(@class, 'groups_post-w')]";
    private const string TopicUrlPath = "//div[@class='media-text_cnt_tx emoji-tx textWrap']";
    private readonly List<HtmlNode> _nodes = [];

    public IReadOnlyCollection<HtmlNode> Nodes => _nodes;

    public OkTopicNodes(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        FillNodes(document);
    }

    private void FillNodes(HtmlDocument document)
    {
        try
        {
            var nodes = document.DocumentNode.SelectNodes(TopicPaths);
            if (nodes == null)
                return;
            foreach (var node in nodes)
            {
                var topicDocument = new HtmlDocument();
                topicDocument.LoadHtml(node.InnerHtml);
                if (topicDocument.DocumentNode.SelectSingleNode(TopicUrlPath) != null)
                    _nodes.Add(topicDocument.DocumentNode);
            }
        }
        catch
        {
            _nodes.Clear();
        }
    }
}
