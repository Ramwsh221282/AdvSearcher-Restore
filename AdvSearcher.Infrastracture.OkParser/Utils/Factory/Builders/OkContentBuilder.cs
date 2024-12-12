using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using HtmlAgilityPack;
using RestSharp;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal sealed class OkContentBuilder(
    IOkAdvertisementBuilder<string> builder,
    RestClient client,
    HtmlDocument document
) : IOkAdvertisementBuilder<string>
{
    private const string Path = "//div[@class='media-text_cnt']";

    public async Task<Result<string>> Build()
    {
        var url = await builder.Build();
        if (url.IsFailure)
            return ParserErrors.CantParseContent;

        await CreateHtmlDocument(url);
        var node = document.DocumentNode.SelectSingleNode(Path);

        if (node == null)
            return ParserErrors.CantParseContent;

        return node.InnerText;
    }

    private async Task CreateHtmlDocument(string url)
    {
        var request = new RestRequest(url);
        RestResponse? response = null;
        while (response == null)
        {
            try
            {
                response = await client.ExecuteAsync(request);
            }
            catch
            {
                // ignored
            }
        }
        document.LoadHtml(response.Content);
    }
}
