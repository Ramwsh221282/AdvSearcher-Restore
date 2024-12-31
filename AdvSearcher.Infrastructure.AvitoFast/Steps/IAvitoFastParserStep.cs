namespace AdvSearcher.Infrastructure.AvitoFast.Steps;

internal interface IAvitoFastParserStep
{
    AvitoFastParserPipeline Pipeline { get; }
    IAvitoFastParserStep? Next { get; }
    Task ProcessAsync();
}
