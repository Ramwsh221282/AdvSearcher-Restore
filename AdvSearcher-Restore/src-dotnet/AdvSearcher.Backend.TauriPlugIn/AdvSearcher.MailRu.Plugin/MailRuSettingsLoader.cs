using AdvSearcher.Core.Tools;

namespace AdvSearcher.MailRu.Plugin;

internal sealed record MailRuSettings(string FromEmail, string SmtpKey);

internal sealed class MailRuSettingsLoader
{
    private static readonly string SettingsPath =
        $@"{Environment.CurrentDirectory}\Plugins\Parsers\Options\MAILRU_Settings.txt";

    public Result<MailRuSettings> Load()
    {
        if (!File.Exists(SettingsPath))
            return new Error("No mail ru settings found");
        using StreamReader reader = new StreamReader(SettingsPath);
        string lines = reader.ReadToEnd();
        try
        {
            ReadOnlySpan<string> pairs = lines.Split('\n');
            string smtpKey = pairs[0].Split('\t')[^1].Trim();
            string fromEmail = pairs[1].Split('\t')[^1].Trim();
            return new MailRuSettings(fromEmail, smtpKey);
        }
        catch
        {
            return new Error("Invalid mail ru settings");
        }
    }
}
