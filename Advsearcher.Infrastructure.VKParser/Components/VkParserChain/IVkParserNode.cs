namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain;

internal interface IVkParserNode
{
    Task ExecuteAsync();

    public VkParserPipeLine PipeLine { get; }

    public IVkParserNode? Next { get; }
}
