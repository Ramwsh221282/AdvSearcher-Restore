using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository;

public interface IPublishersRepository
{
    Task<Result<Publisher>> AddPublisher(Publisher publisher);
    Task<Result<IEnumerable<Publisher>>> AddPublishers(IEnumerable<Publisher> publishers);
    Task<Result<Publisher>> RemovePublisher(Publisher publisher);
}
