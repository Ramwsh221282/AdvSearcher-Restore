using AdvSearcher.Infrastructure.CianParser.CianParserChains.PipeLineComponents;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class SetCardsWrapperElementNode : ICianParserChain
{
    private const string CardsWrapperXpath = "_93444fe79c--wrapper--W0WqH";
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardsWrapperElementNode(CianParserPipeLine pipeLine, ICianParserChain? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.Provider.Instance == null)
            return;
        IWebElement? element = _pipeLine.Provider.Instance.FindElement(
            By.ClassName(CardsWrapperXpath)
        );
        if (element == null)
            return;
        _pipeLine.SetCardsWrapperElement(new CianCardsWrapperElement(element));
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
