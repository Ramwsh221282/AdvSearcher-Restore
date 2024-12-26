using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkHtmlExtractor
{
    private readonly WebDriverProvider _provider;

    public OkHtmlExtractor(WebDriverProvider provider) => _provider = provider;

    public async Task<Result<string>> Extract(ServiceUrl url)
    {
        _provider.InstantiateNewWebDriver();
        if (url.Mode != ServiceUrlMode.Loadable)
            return ParserErrors.UrlIsNotForLoading;
        await ExecuteNavigation(url);
        await ExecuteScrollToBottom();
        Result<string> html = await ExtractHtml();
        _provider.Dispose();
        return html;
    }

    private async Task ExecuteNavigation(ServiceUrl url)
    {
        await new NavigateOnPageCommand(url.Url.Value).ExecuteAsync(_provider);
    }

    private async Task ExecuteScrollToBottom()
    {
        await new ScrollToBottomCommand().ExecuteAsync(_provider);
    }

    private async Task<Result<string>> ExtractHtml()
    {
        IWebDriverQuery<ExtractHtmlQuery, string> query = new ExtractHtmlQuery();
        string html = await query.ExecuteAsync(_provider);
        return string.IsNullOrWhiteSpace(html)
            ? new Error("Не удалось получить HTML страницы")
            : html;
    }
}
