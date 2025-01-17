using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.VK.Parser;

public enum VKTokens
{
    OAuthToken,
    ServiceToken,
}

public sealed class VkOptionsProvider : IVKOptionsProvider
{
    private readonly IOptionManager _manager;

    public VkOptionsProvider(IOptionManager optionManager) => _manager = optionManager;

    public VKOptions Provide()
    {
        IOptionProcessor reader = _manager.CreateReader("VK_Settings.txt");
        Option SAT = reader
            .Process(new Option("Сервисный ключ приложения ВКонтакте", string.Empty))
            .Result;
        Option OAT = reader
            .Process(new Option("OAuth ключ приложения ВКонтакте", string.Empty))
            .Result;
        return new VKOptions(SAT.Value, OAT.Value);
    }
}
