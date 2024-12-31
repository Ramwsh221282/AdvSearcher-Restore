namespace AdvSearcher.Parser.SDK.Option.Extension;

internal sealed class WriteNewFileStrategy : IWriteStrategy
{
    private readonly TxtOptionLogger _logger;

    public WriteNewFileStrategy(TxtOptionLogger logger) => _logger = logger;

    public void Invoke(string _fileName, Options.Option option)
    {
        _logger.Log("Write in new file strategy resolved");
        using (var fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(option.BuildStringRepresentation());
                _logger.Log($"New data has been written key: {option.Key} value: {option.Value}");
            }
        }
    }
}
