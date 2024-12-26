using AdvSearcher.Core.Tools;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateRequestParametersNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateRequestParametersNode(VkParserPipeLine pipeLine, IVkParserNode? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.ServiceUrl == null)
            return;
        Result<VkRequestParameters> parameters = VkRequestParameters.Create(_pipeLine.ServiceUrl);
        _pipeLine.SetParameters(parameters.Value);
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
