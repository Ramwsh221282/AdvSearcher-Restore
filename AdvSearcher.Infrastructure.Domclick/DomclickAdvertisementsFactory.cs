using System.Diagnostics;
using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver;
using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Commands.NavigateToAdvertisementPage;
using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries.ExtractPhoneNumber;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Domclick;

internal sealed class DomclickAdvertisementsFactory(WebDriverProvider provider)
{
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task Construct(IEnumerable<DomclickFetchResult> fetchedResults)
    {
        StartFakeProcess();
        using IWebDriver driver = provider.BuildWebDriver();
        foreach (var item in fetchedResults)
        {
            await ExecuteNavigationOnAdvertisementPage(item, driver);
            Result<string> phoneNumber = await TryExtractPhoneNumber(driver);
            if (phoneNumber.IsFailure)
                continue;
            item.PhoneNumber = phoneNumber.Value;
            CreateParserResponse(item);
        }
    }

    private async Task ExecuteNavigationOnAdvertisementPage(
        DomclickFetchResult result,
        IWebDriver driver
    )
    {
        NavigateToAdvertisementPageCommand command = new NavigateToAdvertisementPageCommand(result);
        await command.ExecuteAsync(driver);
    }

    private async Task<Result<string>> TryExtractPhoneNumber(IWebDriver driver)
    {
        Result<string> phoneNumber = await new ExtractPhoneNumberQuery().ExecuteAsync(driver);
        return phoneNumber;
    }

    private void StartFakeProcess()
    {
        const string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        Process process = Process.Start(
            chromePath,
            "https://krasnoyarsk.domclick.ru/pokupka/kvartiry/krasnoyarskij-kraj/lesosibirsk"
        );
        Thread.Sleep(5000);
        process.Kill();
    }

    private void CreateParserResponse(DomclickFetchResult fetchResult)
    {
        Result<IParsedAdvertisement> advertisement = DomclickAdvertisement.Create(fetchResult);
        if (advertisement.IsFailure)
            return;
        Result<IParsedPublisher> publisher = DomclickPublisher.Create(fetchResult);
        if (publisher.IsFailure)
            return;
        IParsedAttachment[] attachments = DomclickAttachment.Create(fetchResult);
        _results.Add(new DomclickParserResponse(advertisement.Value, attachments, publisher.Value));
    }
}
