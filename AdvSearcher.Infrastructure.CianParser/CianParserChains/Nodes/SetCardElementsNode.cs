using AdvSearcher.Infrastructure.CianParser.CianParserChains.PipeLineComponents;
using AdvSearcher.Parser.SDK;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class SetCardElementsNode : ICianParserChain
{
    private const string CardComponentsXPath = ".//article[@data-name='CardComponent']";
    private readonly CianParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardElementsNode(
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
        _logger.Log("Setting card elements node.");
        if (_pipeLine.WrapperElement == null)
        {
            _logger.Log("Card elements wrapper was not instantiated. Stopping process.");
            return;
        }
        IEnumerable<IWebElement>? cardElements = _pipeLine.WrapperElement.Element.FindElements(
            By.XPath(CardComponentsXPath)
        );
        if (cardElements == null)
        {
            _logger.Log("Advertisement card elements were not found. Stopping process.");
            return;
        }
        _pipeLine.SetCardElements(cardElements.Select(el => new CianCardElement(el)).ToArray());
        _logger.Log("Card elements were set.");
        if (Next != null)
        {
            _logger.Log("Starting next process.");
            await Next.ExecuteAsync();
        }
    }
}
