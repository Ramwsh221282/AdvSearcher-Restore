using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.Abstractions;

public interface IAdvertisementDateConverter<TService>
    where TService : class
{
    Result<DateOnly> Convert(string? stringDate);
}
