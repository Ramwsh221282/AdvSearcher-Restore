using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using HtmlAgilityPack;
using RestSharp;

namespace AdvSearcher.OK.Parser.Utils.Factory.Builders;

public sealed class OkContentBuilder(
    IOkAdvertisementBuilder<string> builder,
    IHttpClient client,
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
        IHttpRequest request = new OkGetContentRequest(url);
        RestResponse? response = null;
        while (response == null)
        {
            try
            {
                response = await client.Instance.ExecuteAsync(request.Request);
            }
            catch
            {
                // ignored
            }
        }
        document.LoadHtml(response.Content);
    }
}

internal sealed class OkGetContentRequest : IHttpRequest
{
    public RestRequest Request => _request;
    private readonly RestRequest _request;

    public OkGetContentRequest(string url)
    {
        _request = new RestRequest(url);
    }
}
