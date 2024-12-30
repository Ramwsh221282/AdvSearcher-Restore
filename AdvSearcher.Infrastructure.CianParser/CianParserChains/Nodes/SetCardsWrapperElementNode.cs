using AdvSearcher.Infrastructure.CianParser.CianParserChains.PipeLineComponents;
using AdvSearcher.Parser.SDK;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class SetCardsWrapperElementNode : ICianParserChain
{
    private const string CardsWrapperXpath = "_93444fe79c--wrapper--W0WqH";
    private readonly ParserConsoleLogger _logger;
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardsWrapperElementNode(
        CianParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Setting cards wrapper element node");
        if (_pipeLine.Provider.Instance == null)
        {
            _logger.Log("Web driver provider was not instantiaded. Stopping process.");
            return;
        }
        IWebElement? element = _pipeLine.Provider.Instance.FindElement(
            By.ClassName(CardsWrapperXpath)
        );
        if (element == null)
        {
            _logger.Log("Cards wrapper element was not found. Stopping process.");
            return;
        }
        _pipeLine.SetCardsWrapperElement(new CianCardsWrapperElement(element));
        _logger.Log("Cards wrapper element instantiated");
        if (Next != null)
        {
            _logger.Log("Processing next chain node");
            await Next.ExecuteAsync();
        }
    }
}
