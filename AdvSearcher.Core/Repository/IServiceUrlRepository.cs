using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository;

public interface IServiceUrlRepository
{
    Task<Result<ServiceUrl>> AddServiceUrlAsync(ServiceUrl url);
    Task<Result<ServiceUrl>> DropServiceUrlAsync(ServiceUrl url);
    Task<IEnumerable<ServiceUrl>> GetServiceUrlsByMode(ServiceUrlMode mode);
}
