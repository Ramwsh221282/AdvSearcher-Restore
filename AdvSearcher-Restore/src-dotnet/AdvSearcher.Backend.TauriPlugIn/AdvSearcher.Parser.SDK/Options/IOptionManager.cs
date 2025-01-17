namespace AdvSearcher.Parser.SDK.Options;

public interface IOptionManager
{
    IOptionProcessor CreateReader(string fileName);
    IOptionProcessor CreateWrite(string fileName);
    IOptionProcessor CreateFlusher(string fileName);
}
