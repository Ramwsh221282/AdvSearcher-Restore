using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository.InMemoryRepository;

public sealed class InMemoryPublishersRepository : IPublishersRepository
{
    private readonly List<Publisher> _publishers = [];

    public async Task<Result<Publisher>> AddPublisher(Publisher publisher)
    {
        if (
            _publishers.Any(p =>
                p.PublisherId == publisher.PublisherId
                && p.Name == publisher.Name
                && p.PhoneNumber == publisher.PhoneNumber
            )
        )
            return new Error("Дубликат автора");
        _publishers.Add(publisher);
        return await Task.FromResult(publisher);
    }

    public async Task<Result<IEnumerable<Publisher>>> AddPublishers(
        IEnumerable<Publisher> publishers
    )
    {
        foreach (var publisher in publishers)
        {
            if (
                _publishers.Any(p =>
                    p.PublisherId == publisher.PublisherId
                    && p.Name == publisher.Name
                    && p.PhoneNumber == publisher.PhoneNumber
                )
            )
                continue;
            _publishers.Add(publisher);
        }

        return await Task.FromResult(Result<IEnumerable<Publisher>>.Success(_publishers));
    }

    public async Task<Result<Publisher>> RemovePublisher(Publisher publisher)
    {
        var removed = _publishers.Remove(publisher);
        if (!removed)
            return new Error("Автор не был найден");
        return await Task.FromResult(publisher);
    }
}
