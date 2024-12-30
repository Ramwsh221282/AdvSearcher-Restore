using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateRequestParametersNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateRequestParametersNode(
        VkParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Creating request parameters");
        if (_pipeLine.ServiceUrl == null)
        {
            _logger.Log("Vk Parse Url was not provided. Stopping process.");
            return;
        }
        Result<VkRequestParameters> parameters = VkRequestParameters.Create(_pipeLine.ServiceUrl);
        if (parameters.IsFailure)
        {
            _logger.Log($"Failed to create request parameters. Reason: {parameters.Error}");
            return;
        }
        _pipeLine.SetParameters(parameters.Value);
        _logger.Log("Vk Request parameters were initialized.");
        if (Next != null)
        {
            _logger.Log("Processing next step.");
            await Next.ExecuteAsync();
        }
    }
}
