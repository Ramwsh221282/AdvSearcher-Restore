using AdvSearcher.Core.Tools;

namespace AdvSearcher.OkPublishing.Plugin;

internal sealed record OkTokens(string PublicToken, string LongToken);

internal sealed class OkTokensLoader
{
    private static readonly string SettingsPath =
        $@"{Environment.CurrentDirectory}\Plugins\Parsers\Options\OK_Settings.txt";

    public Result<OkTokens> Load()
    {
        if (!File.Exists(SettingsPath))
            return new Error("Ok settings were not found");
        using var reader = new StreamReader(SettingsPath);
        string lines = reader.ReadToEnd();
        try
        {
            ReadOnlySpan<string> pairs = lines.Split('\n');
            string publictoken = pairs[0].Split('\t')[^1].Trim();
            string longToken = pairs[1].Split('\t')[^1].Trim();
            return new OkTokens(publictoken, longToken);
        }
        catch
        {
            return new Error("Invalid OK Settings");
        }
    }
}
