using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Persistance.SDK;

public interface IPublishersRepository
{
    Task<Result<RepositoryOperationResult>> Remove(Publisher publisher);
    Task<Result<RepositoryOperationResult>> Add(Publisher publisher);
    Task<IEnumerable<Publisher>> GetAll();
}
