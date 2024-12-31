using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class OptionWriter : IOptionProcessor
{
    private readonly string _fileName;
    private readonly PathManager _manager;
    private readonly TxtOptionLogger _logger;

    public OptionWriter(string fileName, PathManager manager, TxtOptionLogger logger)
    {
        _fileName = fileName;
        _manager = manager;
        _logger = logger;
    }

    public async Task<Options.Option> Process(Options.Option option)
    {
        string path = _manager.CreatePath(_fileName);
        _logger.Log($"Writing in path: {path}");
        GetStrategy().Invoke(path, option);
        return await Task.FromResult(option);
    }

    private IWriteStrategy GetStrategy() =>
        new WriterStrategyResolver(_fileName, _manager, _logger).Resolve();
}
