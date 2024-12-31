using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class OptionReader : IOptionProcessor
{
    private readonly string _fileName;
    private readonly PathManager manager;
    private readonly TxtOptionLogger _logger;

    public OptionReader(string fileName, PathManager _manager, TxtOptionLogger logger)
    {
        _fileName = fileName;
        manager = _manager;
        _logger = logger;
    }

    public async Task<Options.Option> Process(Options.Option option)
    {
        string path = manager.CreatePath(_fileName);
        _logger.Log($"Reading from: {path}");
        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new StreamReader(fileStream))
            {
                string lines = await reader.ReadToEndAsync();
                if (string.IsNullOrEmpty(lines))
                {
                    _logger.Log($"No data found with key: {option.Key} and value: {option.Value}");
                    return new Options.Option(string.Empty, string.Empty);
                }
                _logger.Log($"Found: {option.Key} and value: {option.Value}");
                return GetKeyValuePair(lines, option);
            }
        }
    }

    private Options.Option GetKeyValuePair(string lines, Options.Option option)
    {
        ReadOnlySpan<string> splittedLines = lines.Split(
            "\r\n",
            StringSplitOptions.RemoveEmptyEntries
        );
        for (int index = 0; index < splittedLines.Length; index++)
        {
            ReadOnlySpan<string> currentKeyValuePair = splittedLines[index]
                .Split("\t", StringSplitOptions.RemoveEmptyEntries);
            for (int subbindex = 0; subbindex < currentKeyValuePair.Length; subbindex++)
            {
                if (currentKeyValuePair[subbindex] == option.Key)
                    return new Options.Option(
                        currentKeyValuePair[subbindex],
                        currentKeyValuePair[^1]
                    );
            }
        }
        return new Options.Option(string.Empty, string.Empty);
    }
}
