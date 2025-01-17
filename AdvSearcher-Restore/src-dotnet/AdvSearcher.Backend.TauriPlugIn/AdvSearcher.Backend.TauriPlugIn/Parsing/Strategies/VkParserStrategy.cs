using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;

public sealed class VkParserStrategy : IParsingStrategy
{
    private readonly List<ParserFilterOption> _options;
    private readonly PersistanceServiceFactory _factory;
    private readonly ParserResolver _resolver;
    private const string ParserName = "VkParser";

    public VkParserStrategy(
        ParsingOptions options,
        ParserResolver resolver,
        PersistanceServiceFactory factory
    )
    {
        _resolver = resolver;
        _factory = factory;
        _options = options.CreateOptions(_factory);
    }

    public async Task Perform(
        Action<int> currentProgress,
        Action<int> maxProgress,
        Action<string> notificationsPublisher
    )
    {
        IEnumerable<ServiceUrl> urls = await _factory.GetAllLoadableServiceUrlsOfService("VK");
        foreach (ServiceUrl url in urls)
        {
            IParser parser = _resolver.Resolve(ParserName);
            parser.SetCurrentProgressValuePublisher(currentProgress);
            parser.SetMaxProgressValuePublisher(maxProgress);
            parser.SetNotificationPublisher(notificationsPublisher);
            await parser.ParseData(url, _options);
            IEnumerable<Advertisement> advertisements = parser.Results.ToAdvertisements();
            await _factory.AppendAdvertisementsCollectionInRepository(advertisements);
            maxProgress.Invoke(0);
            currentProgress.Invoke(0);
        }
    }
}
