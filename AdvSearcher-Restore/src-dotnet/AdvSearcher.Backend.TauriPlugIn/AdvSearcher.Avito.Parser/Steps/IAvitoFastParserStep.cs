namespace AdvSearcher.Avito.Parser.Steps;

public interface IAvitoFastParserStep
{
    AvitoFastParserPipeline Pipeline { get; }
    IAvitoFastParserStep? Next { get; }
    Task ProcessAsync();
}
