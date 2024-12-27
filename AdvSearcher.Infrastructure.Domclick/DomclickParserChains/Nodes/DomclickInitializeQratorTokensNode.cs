namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeQratorTokensNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeLine;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeLine;

    public DomclickInitializeQratorTokensNode(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeLine = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        if (_pipeLine.Provider.Instance == null)
            return;
        string qratorValue = CreateQratorTokensValue();
        if (string.IsNullOrWhiteSpace(qratorValue))
            return;
        _pipeLine.SetQratorValue(qratorValue);
        if (Next != null)
            await Next.Process();
    }

    private string CreateQratorTokensValue()
    {
        string qratorValue = string.Empty;
        while (string.IsNullOrWhiteSpace(qratorValue))
        {
            DomclickQratorSsid2Factory factory = new DomclickQratorSsid2Factory(_pipeLine.Provider);
            //DomclickQratorCookies? ssid2 = factory.CreateSsid2();
            DomclickQratorCookies? jsid2 = factory.CreateJsid2();
            if (jsid2 == null)
                continue;
            qratorValue = factory.CreateValues(jsid2);
        }
        return qratorValue;
    }
}
