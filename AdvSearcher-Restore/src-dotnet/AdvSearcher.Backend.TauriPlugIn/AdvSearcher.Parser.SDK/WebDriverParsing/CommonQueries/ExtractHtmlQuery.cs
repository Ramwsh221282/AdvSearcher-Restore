namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

public sealed class ExtractHtmlQuery : IWebDriverQuery<string>
{
    public async Task<string> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return string.Empty;

        string? html = provider.Instance.PageSource;
        return string.IsNullOrEmpty(html)
            ? await Task.FromResult(string.Empty)
            : await Task.FromResult(html);
    }
}
