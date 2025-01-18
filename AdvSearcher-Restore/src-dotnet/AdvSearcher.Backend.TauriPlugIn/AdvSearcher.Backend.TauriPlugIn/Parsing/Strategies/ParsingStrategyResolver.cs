using AdvSearcher.MachineLearning.SDK;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Persistance.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;

public class ParsingStrategyResolver
{
    private readonly ParsingRequest _request;
    private readonly ParserResolver _resolver;
    private readonly PersistanceServiceFactory _factory;
    private readonly ISpamClassifier _spamClassifier;

    public ParsingStrategyResolver(
        ParsingRequest request,
        ParserResolver resolver,
        PersistanceServiceFactory factory,
        ISpamClassifier spamClassifier
    )
    {
        _request = request;
        _resolver = resolver;
        _factory = factory;
        _spamClassifier = spamClassifier;
    }

    public IParsingStrategy Resolve()
    {
        string service = _request.ServiceName;
        return service switch
        {
            null => throw new ArgumentNullException(),
            not null when string.IsNullOrWhiteSpace(service) => throw new ArgumentNullException(),
            not null when service.Contains("VK", StringComparison.OrdinalIgnoreCase) =>
                new VkParserStrategy(_request.Options, _resolver, _factory, _spamClassifier),
            not null when service.Contains("OK", StringComparison.OrdinalIgnoreCase) =>
                new OkParserStrategy(_request.Options, _resolver, _factory, _spamClassifier),
            not null when service.Contains("Cian", StringComparison.OrdinalIgnoreCase) =>
                new CianParserStrategy(_request.Options, _resolver, _factory),
            not null when service.Contains("Avito", StringComparison.OrdinalIgnoreCase) =>
                new AvitoParserStrategy(_request.Options, _resolver, _factory),
            not null when service.Contains("Domclick", StringComparison.OrdinalIgnoreCase) =>
                new DomclickParserStrategy(_request.Options, _resolver, _factory),
            _ => throw new NotSupportedException(),
        };
    }
}
