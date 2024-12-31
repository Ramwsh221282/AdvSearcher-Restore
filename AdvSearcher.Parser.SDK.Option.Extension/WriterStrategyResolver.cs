namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class WriterStrategyResolver
{
    private readonly string _fileName;
    private readonly PathManager _manager;
    private readonly TxtOptionLogger _logger;

    public WriterStrategyResolver(string fileName, PathManager manager, TxtOptionLogger logger)
    {
        _fileName = fileName;
        _manager = manager;
        _logger = logger;
    }

    public IWriteStrategy Resolve()
    {
        string path = _manager.CreatePath(_fileName);
        bool existance = File.Exists(path);
        return existance switch
        {
            true => new WriteInExistingFileStrategy(_logger),
            false => new WriteNewFileStrategy(_logger),
        };
    }
}
