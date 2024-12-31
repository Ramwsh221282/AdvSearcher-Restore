namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class PathManager
{
    private static readonly string StaticPath =
        $@"{AppDomain.CurrentDomain.BaseDirectory}\Plugins\Parsers\Options";

    public PathManager()
    {
        if (!Directory.Exists(StaticPath))
            Directory.CreateDirectory(StaticPath);
    }

    public string CreatePath(string path) => Path.Combine(StaticPath, path);
}
