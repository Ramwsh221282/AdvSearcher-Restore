namespace AdvSearcher.VK.Parser.Components.VkParserChain;

public interface IVkParserNode
{
    Task ExecuteAsync();

    public VkParserPipeLine PipeLine { get; }

    public IVkParserNode? Next { get; }
}
