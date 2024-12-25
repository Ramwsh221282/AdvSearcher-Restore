namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

public sealed class NavigateOnPageCommand
    : WebDriverCommand,
        IWebDriverCommand<NavigateOnPageCommand>
{
    private readonly string _url;

    public NavigateOnPageCommand(string url) => _url = url;

    public override async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;

        await provider.Instance.Navigate().GoToUrlAsync(_url);
    }
}
