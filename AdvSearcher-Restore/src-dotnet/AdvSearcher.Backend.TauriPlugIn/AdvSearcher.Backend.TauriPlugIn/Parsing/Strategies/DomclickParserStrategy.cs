using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;

public sealed class DomclickParserStrategy : IParsingStrategy
{
    private readonly List<ParserFilterOption> _options;
    private readonly PersistanceServiceFactory _factory;
    private readonly ParserResolver _resolver;
    private const string ParserName = "DomclickParser";

    public DomclickParserStrategy(
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
        IParser parser = _resolver.Resolve(ParserName);
        parser.SetMaxProgressValuePublisher(maxProgress);
        parser.SetCurrentProgressValuePublisher(currentProgress);
        parser.SetNotificationPublisher(notificationsPublisher);
        await parser.ParseData(null!, _options); // domclick doens't require service url
        IEnumerable<Advertisement> advertisements = parser.Results.ToAdvertisements();
        await _factory.AppendAdvertisementsCollectionInRepository(advertisements);
        maxProgress.Invoke(0);
        currentProgress.Invoke(0);
    }
}
