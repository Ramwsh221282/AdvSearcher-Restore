namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

public sealed class NavigateOnPageCommand : IWebDriverCommand
{
    private readonly string _url;

    public NavigateOnPageCommand(string url) => _url = url;

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        await provider.Instance.Navigate().GoToUrlAsync(_url);
    }
}
