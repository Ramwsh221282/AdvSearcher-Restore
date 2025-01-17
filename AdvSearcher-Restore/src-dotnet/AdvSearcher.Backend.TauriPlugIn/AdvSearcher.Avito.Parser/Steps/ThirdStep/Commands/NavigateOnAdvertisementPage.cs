using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

namespace AdvSearcher.Avito.Parser.Steps.ThirdStep.Commands;

public sealed class NavigateOnAdvertisementPage : IWebDriverCommand
{
    private readonly AvitoAdvertisement _advertisement;

    public NavigateOnAdvertisementPage(AvitoAdvertisement advertisement) =>
        _advertisement = advertisement;

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        await new NavigateOnPageCommand(_advertisement.SourceUrl).ExecuteAsync(provider);
        await new ScrollToBottomCommand().ExecuteAsync(provider);
        await new ScrollToTopCommand().ExecuteAsync(provider);
    }
}
