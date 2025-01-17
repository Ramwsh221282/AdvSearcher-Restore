using AdvSearcher.Core.Tools;

namespace AdvSearcher.VkPublishing.Plugin;

internal sealed record VkPublishingTokens(string ServiceAccessToken, string OAuthAccessToken);

internal sealed class VkPublishingTokensLoader
{
    private readonly string _file =
        @$"{Environment.CurrentDirectory}\Plugins\Parsers\Options\VK_Settings.txt";

    public Result<VkPublishingTokens> Load()
    {
        if (!File.Exists(_file))
            return new Error("No VK tokens provided");
        using var reader = new StreamReader(_file);
        string lines = reader.ReadToEnd();
        try
        {
            ReadOnlySpan<string> pairs = lines.Split('\n');
            string serviceAccessToken = pairs[0].Split('\t')[^1].Trim();
            string oauthAccessToken = pairs[1].Split('\t')[^1].Trim();
            return new VkPublishingTokens(serviceAccessToken, oauthAccessToken);
        }
        catch
        {
            return new Error("Invalid Vk Settings file");
        }
    }
}
