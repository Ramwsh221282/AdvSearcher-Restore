using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal interface IAvitoDateConverterComponent
{
    IAvitoDateConverterComponent? Next { get; }
    public Result<DateOnly> Convert(string stringDate);
}
