using AdvSearcher.Core.Tools;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;

public interface IAvitoDateConverterComponent
{
    IAvitoDateConverterComponent? Next { get; }
    public Result<DateOnly> Convert(string stringDate);
}
