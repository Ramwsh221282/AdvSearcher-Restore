using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Persistance.SDK;

public sealed record ServiceUrlRequestOption(ServiceUrlMode mode, ServiceUrlService service);

public interface IServiceUrlRepository
{
    Task<Result<RepositoryOperationResult>> Add(ServiceUrl serviceUrl);
    Task<Result<RepositoryOperationResult>> Remove(ServiceUrl serviceUrl);
    Task<IEnumerable<ServiceUrl>> Get(ServiceUrlMode mode, ServiceUrlService service);
}
