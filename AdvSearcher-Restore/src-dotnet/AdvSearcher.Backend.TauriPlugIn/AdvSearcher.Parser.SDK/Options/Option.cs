using System.Text;

namespace AdvSearcher.Parser.SDK.Options;

public record Option(string Key, string Value)
{
    public ReadOnlySpan<char> BuildStringRepresentation()
    {
        if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(Value))
            return string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Key);
        stringBuilder.Append("\t");
        stringBuilder.Append(Value);
        return stringBuilder.ToString().AsSpan();
    }
}
