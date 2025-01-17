using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Parser.SDK;

public sealed class ParserProvider
{
    private readonly IParser[] _parsers;

    public ParserProvider(IEnumerable<IParser> parsers)
    {
        _parsers = parsers.ToArray();
    }

    public IParser GetParser(string parserName)
    {
        IParser? parser = _parsers.FirstOrDefault(p => p.GetType().Name == parserName);
        if (parser == null)
            throw new Exception($"No parser found with name: {parserName}");
        return parser;
    }
}
