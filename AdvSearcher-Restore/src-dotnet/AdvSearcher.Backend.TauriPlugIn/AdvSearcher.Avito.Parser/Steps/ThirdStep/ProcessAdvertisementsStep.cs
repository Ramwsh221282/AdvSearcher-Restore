using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Avito.Parser.Steps.ThirdStep.Commands;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Avito.Parser.Steps.ThirdStep;

public sealed class ProcessAdvertisementsStep : IAvitoFastParserStep
{
    private readonly WebDriverProvider _driver;
    private int _currentProgress;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public ProcessAdvertisementsStep(
        AvitoFastParserPipeline pipeLine,
        WebDriverProvider driver,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeLine;
        Next = next;
        _driver = driver;
    }

    public async Task ProcessAsync()
    {
        if (_driver.Instance == null)
            return;
        if (Pipeline.Total == 0)
            return;
        Pipeline.FilterByDate();
        Pipeline.FilterByCache();
        Pipeline.MaxProgressPublisher?.Invoke(Pipeline.Count());
        foreach (AvitoAdvertisement advertisement in Pipeline)
        {
            await new NavigateOnAdvertisementPage(advertisement).ExecuteAsync(_driver);
            bool isHomeOwner = await new IsHomeOwnerQuery().ExecuteAsync(_driver);
            if (!isHomeOwner)
            {
                advertisement.IsAgent = true;
                _currentProgress++;
                Pipeline.CurrentProgressPublisher?.Invoke(_currentProgress);
            }
            await new ProcessAdvertisementDescription(advertisement).ExecuteAsync(_driver);
            await new ProcessAdvertisementPublisher(advertisement).ExecuteAsync(_driver);
            _currentProgress++;
            Pipeline.CurrentProgressPublisher?.Invoke(_currentProgress);
        }
        Pipeline.CleanFromAgents();
        _driver.Dispose();
        if (Next != null)
            await Next.ProcessAsync();
    }
}
