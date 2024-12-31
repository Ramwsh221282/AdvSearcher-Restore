using AdvSearcher.Parser.SDK.Options;

namespace Advsearcher.Infrastructure.VKParser;

internal enum VKTokens
{
    OAuthToken,
    ServiceToken,
}

internal sealed class VkOptionsProvider : IVkOptionsProvider
{
    private readonly IOptionManager _manager;

    public VkOptionsProvider(IOptionManager optionManager) => _manager = optionManager;

    public VkOptions Provide()
    {
        IOptionProcessor reader = _manager.CreateReader("VK_TOKENS.txt");
        Option SAT = reader.Process(new Option(nameof(VKTokens.ServiceToken), string.Empty)).Result;
        Option OAT = reader.Process(new Option(nameof(VKTokens.OAuthToken), string.Empty)).Result;
        return new VkOptions(SAT.Value, OAT.Value);
    }
}
