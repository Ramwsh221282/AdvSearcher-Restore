using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal interface IAvitoDateConverterComponent
{
    IAvitoDateConverterComponent? Next { get; }
    public Result<DateOnly> Convert(string stringDate);
}
