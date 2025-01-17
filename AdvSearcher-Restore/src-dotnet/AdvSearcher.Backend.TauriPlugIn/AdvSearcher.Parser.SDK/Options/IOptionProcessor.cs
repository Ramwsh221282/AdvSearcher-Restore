namespace AdvSearcher.Parser.SDK.Options;

public interface IOptionProcessor
{
    Task<Option> Process(Option option);
}
