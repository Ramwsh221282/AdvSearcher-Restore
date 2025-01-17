using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;

public sealed class AvitoParserStrategy : IParsingStrategy
{
    private readonly List<ParserFilterOption> _options;
    private readonly ParserResolver _resolver;
    private readonly PersistanceServiceFactory _factory;
    private const string ParserName = "AvitoFastParser";

    public AvitoParserStrategy(
        ParsingOptions options,
        ParserResolver resolver,
        PersistanceServiceFactory factory
    )
    {
        _factory = factory;
        _resolver = resolver;
        _options = options.CreateOptions(_factory);
    }

    public async Task Perform(
        Action<int> currentProgress,
        Action<int> maxProgress,
        Action<string> notificationsPublisher
    )
    {
        IEnumerable<ServiceUrl> urls = await _factory.GetAllLoadableServiceUrlsOfService("AVITO");
        foreach (ServiceUrl url in urls)
        {
            IParser parser = _resolver.Resolve(ParserName);
            parser.SetMaxProgressValuePublisher(maxProgress);
            parser.SetCurrentProgressValuePublisher(currentProgress);
            parser.SetNotificationPublisher(notificationsPublisher);
            await parser.ParseData(url, _options);
            IEnumerable<Advertisement> advertisements = parser.Results.ToAdvertisements();
            await _factory.AppendAdvertisementsCollectionInRepository(advertisements);
            maxProgress.Invoke(0);
            currentProgress.Invoke(0);
            break; // avito doesn't require more than 1 url for parsing
        }
    }
}
