using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Publishers;

public class Publisher
{
    public delegate Task<Result<Publisher>> PublisherCreatedHandler(Publisher publisher);
    private event PublisherCreatedHandler? OnPublisherCreated;

    public int Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public ulong PublisherId { get; private set; }
    public string PhoneNumber { get; private set; } = string.Empty;

    public static Result<Publisher> Create(string name, string phoneNumber = "", ulong id = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Error("Имя было пустым");
        var publisher = new Publisher();
        publisher.Name = name;
        if (id != default)
            publisher.PublisherId = id;
        if (!string.IsNullOrEmpty(phoneNumber))
            publisher.PhoneNumber = phoneNumber;
        return publisher;
    }

    public static async Task<Result<Publisher>> Create(
        string name,
        PublisherCreatedHandler publisherCreatedHandler,
        ulong id = default,
        string phoneNumber = ""
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Error("Имя было пустым");
        var publisher = new Publisher();
        publisher.Name = name;
        if (id != default)
            publisher.PublisherId = id;
        if (!string.IsNullOrEmpty(phoneNumber))
            publisher.PhoneNumber = phoneNumber;
        publisher.OnPublisherCreated += publisherCreatedHandler;
        var result = await publisher.OnPublisherCreated.Invoke(publisher);
        publisher.OnPublisherCreated -= publisherCreatedHandler;
        return result;
    }
}
