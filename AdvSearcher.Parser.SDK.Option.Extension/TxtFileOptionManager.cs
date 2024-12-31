using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class TxtFileOptionManager : IOptionManager
{
    private readonly PathManager _pathManager;
    private readonly TxtOptionLogger _logger;

    public TxtFileOptionManager(PathManager pathManager, TxtOptionLogger logger)
    {
        _pathManager = pathManager;
        _logger = logger;
    }

    public IOptionProcessor CreateReader(string fileName) =>
        new OptionReader(fileName, _pathManager, _logger);

    public IOptionProcessor CreateWrite(string fileName) =>
        new OptionWriter(fileName, _pathManager, _logger);

    public IOptionProcessor CreateFlusher(string fileName) =>
        new OptionFlusher(fileName, _pathManager, _logger);
}
