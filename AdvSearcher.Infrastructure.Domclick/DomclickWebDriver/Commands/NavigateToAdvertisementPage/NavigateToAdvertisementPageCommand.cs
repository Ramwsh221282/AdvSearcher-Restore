using AdvSearcher.Infrastructure.Domclick.InternalModels;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Commands.NavigateToAdvertisementPage;

internal sealed class NavigateToAdvertisementPageCommand
    : IDomclickWebDriverCommand<NavigateToAdvertisementPageCommand>
{
    private readonly DomclickFetchResult _result;

    public NavigateToAdvertisementPageCommand(DomclickFetchResult result) => _result = result;

    public async Task ExecuteAsync(IWebDriver driver)
    {
        if (string.IsNullOrEmpty(_result.SourceUrl))
            return;

        await driver.Navigate().GoToUrlAsync(_result.SourceUrl);
    }
}
