using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.PipeLineComponents;

internal sealed class SetCardsWrapperElementNode : ICianParserChain
{
    private const string CardsWrapperXpath = "_93444fe79c--wrapper--W0WqH";
    private readonly WebDriverProvider _provider;
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public SetCardsWrapperElementNode(
        CianParserPipeLine pipeLine,
        WebDriverProvider provider,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _provider = provider;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_provider.Instance == null)
            return;
        IWebElement? element = _provider.Instance.FindElement(By.XPath(CardsWrapperXpath));
        if (element == null)
            return;
        _pipeLine.SetCardsWrapperElement(new CianCardsWrapperElement(element));
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
