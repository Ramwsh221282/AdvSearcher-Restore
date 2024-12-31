using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep.Commands;

internal sealed class NavigateOnAdvertisementPage : IWebDriverCommand<NavigateOnAdvertisementPage>
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
