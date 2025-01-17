namespace AdvSearcher.Parser.SDK.Option.Extension;

internal interface IWriteStrategy
{
    void Invoke(string _fileName, Options.Option option);
}
