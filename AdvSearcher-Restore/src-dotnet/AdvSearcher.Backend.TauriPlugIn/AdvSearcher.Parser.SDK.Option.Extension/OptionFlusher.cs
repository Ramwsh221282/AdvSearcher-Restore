using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class OptionFlusher : IOptionProcessor
{
    private readonly string _fileName;
    private readonly PathManager _manager;
    private readonly TxtOptionLogger _logger;

    public OptionFlusher(string fileName, PathManager manager, TxtOptionLogger logger)
    {
        _fileName = fileName;
        _manager = manager;
        _logger = logger;
    }

    public async Task<Options.Option> Process(Options.Option option)
    {
        string path = _manager.CreatePath(_fileName);
        _logger.Log($"Removing file in {path}");
        bool existance = File.Exists(path);
        return existance switch
        {
            true => await Task.FromResult(RemoveAndReturnEmpty(path)),
            false => await Task.FromResult(option),
        };
    }

    private Options.Option RemoveAndReturnEmpty(string path)
    {
        File.Delete(path);
        _logger.Log($"Removed file in {path}");
        return new Options.Option(string.Empty, string.Empty);
    }
}
