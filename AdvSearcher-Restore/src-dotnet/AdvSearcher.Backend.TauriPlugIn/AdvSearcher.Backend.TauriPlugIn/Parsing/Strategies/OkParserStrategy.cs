using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.MachineLearning.SDK;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;

public sealed class OkParserStrategy : IParsingStrategy
{
    private readonly List<ParserFilterOption> _options;
    private readonly PersistanceServiceFactory _factory;
    private readonly ParserResolver _resolver;
    private const string ParserName = "OkParser";
    private readonly ISpamClassifier _spamClassifier;

    public OkParserStrategy(
        ParsingOptions options,
        ParserResolver resolver,
        PersistanceServiceFactory factory,
        ISpamClassifier spamClassifier
    )
    {
        _factory = factory;
        _resolver = resolver;
        _options = options.CreateOptions(factory);
        _spamClassifier = spamClassifier;
    }

    public async Task Perform(
        Action<int> currentProgress,
        Action<int> maxProgress,
        Action<string> notificationsPublisher
    )
    {
        IEnumerable<ServiceUrl> urls = await _factory.GetAllLoadableServiceUrlsOfService("OK");
        foreach (ServiceUrl url in urls)
        {
            IParser parser = _resolver.Resolve(ParserName);
            parser.SetMaxProgressValuePublisher(maxProgress);
            parser.SetCurrentProgressValuePublisher(currentProgress);
            parser.SetNotificationPublisher(notificationsPublisher);
            await parser.ParseData(url, _options);
            IEnumerable<Advertisement> advertisements = parser.Results.ToAdvertisements();
            advertisements = advertisements.Where(ad => !_spamClassifier.IsSpam(ad));
            await _factory.AppendAdvertisementsCollectionInRepository(advertisements);
            maxProgress.Invoke(0);
            currentProgress.Invoke(0);
        }
    }
}
