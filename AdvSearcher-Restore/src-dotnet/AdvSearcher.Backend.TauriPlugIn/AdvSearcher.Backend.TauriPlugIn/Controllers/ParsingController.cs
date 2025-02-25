using AdvSearcher.Backend.TauriPlugIn.MessageListener;
using AdvSearcher.Backend.TauriPlugIn.Parsing;
using AdvSearcher.Backend.TauriPlugIn.Parsing.Strategies;
using AdvSearcher.MachineLearning.SDK;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Persistance.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public class ParsingController(
    ParserResolver resolver,
    PersistanceServiceFactory factory,
    IEventPublisher publisher,
    ISpamClassifier spamClassifier
)
{
    private const string Progress = "parser-process-progress";
    private const string MaxProgress = "parser-max-progress";
    private readonly ParserResolver _resolver = resolver;
    private readonly PersistanceServiceFactory _factory = factory;
    private readonly IEventPublisher _publisher = publisher;

    public void ProcessParsing(ParsingRequest request)
    {
        try
        {
            Action<int> currentProgress = (value) => _publisher.Publish(Progress, value);
            Action<int> maxProgress = (value) => _publisher.Publish(MaxProgress, value);
            Action<string> notificationsPublisher = (value) =>
                _publisher.Publish(TauriMessageListener.NotificationsListener, value);
            ParsingStrategyResolver resolver = new ParsingStrategyResolver(
                request,
                _resolver,
                _factory,
                spamClassifier
            );
            IParsingStrategy strategy = resolver.Resolve();
            ParsingContext context = new ParsingContext(
                strategy,
                currentProgress,
                maxProgress,
                notificationsPublisher
            );
            context.ProcessParsing().Wait();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}
