using AdvSearcher.Cian.Parser.CianParserChains.PipeLineComponents;
using OpenQA.Selenium;

namespace AdvSearcher.Cian.Parser.CianParserChains.Nodes;

public sealed class SetCardElementsNode : ICianParserChain
{
    private const string CardComponentsXPath = ".//article[@data-name='CardComponent']";
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardElementsNode(CianParserPipeLine pipeLine, ICianParserChain? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.WrapperElement == null)
            return;
        IEnumerable<IWebElement>? cardElements = _pipeLine.WrapperElement.Element.FindElements(
            By.XPath(CardComponentsXPath)
        );
        if (cardElements == null)
            return;
        _pipeLine.SetCardElements(cardElements.Select(el => new CianCardElement(el)).ToArray());
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
