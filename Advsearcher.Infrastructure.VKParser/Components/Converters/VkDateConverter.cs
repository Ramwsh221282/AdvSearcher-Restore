using AdvSearcher.Core.Tools;

namespace Advsearcher.Infrastructure.VKParser.Components.Converters;

internal sealed class VkDateConverter
{
    public Result<DateOnly> Convert(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return new Error("Строка даты была пустой");

        var timestamp = double.Parse(stringDate);
        var date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        return DateOnly.FromDateTime(date);
    }
}
