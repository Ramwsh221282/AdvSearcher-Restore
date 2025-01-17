using AdvSearcher.Core.Tools;

namespace AdvSearcher.WhatsApp.Plugin;

internal sealed record GreenApiTokens(string InstanceId, string Token);

internal sealed class GreenApiTokensLoader
{
    private static readonly string SettingsPath =
        $@"{Environment.CurrentDirectory}\Plugins\Parsers\Options\WA_Settings.txt";

    public Result<GreenApiTokens> Load()
    {
        if (!File.Exists(SettingsPath))
            return new Error("WhatsApp settings do not exist");
        using var reader = new StreamReader(SettingsPath);
        string lines = reader.ReadToEnd();
        try
        {
            ReadOnlySpan<string> pairs = lines.Split('\n');
            string instanceId = pairs[0].Split('\t')[^1].Trim();
            string instanceToken = pairs[1].Split('\t')[^1].Trim();
            return new GreenApiTokens(instanceId, instanceToken);
        }
        catch
        {
            return new Error("Invalid tokens");
        }
    }
}
