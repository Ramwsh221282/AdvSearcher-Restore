namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class WriteInExistingFileStrategy : IWriteStrategy
{
    private readonly TxtOptionLogger _logger;

    public WriteInExistingFileStrategy(TxtOptionLogger logger) => _logger = logger;

    public void Invoke(string _fileName, Options.Option option)
    {
        _logger.Log("Existing file strategy writing resolved");
        using (var fileStream = new FileStream(_fileName, FileMode.Append, FileAccess.Write))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(option.BuildStringRepresentation());
                _logger.Log($"Data has been written key: {option.Key} value: {option.Value}");
            }
        }
    }
}
