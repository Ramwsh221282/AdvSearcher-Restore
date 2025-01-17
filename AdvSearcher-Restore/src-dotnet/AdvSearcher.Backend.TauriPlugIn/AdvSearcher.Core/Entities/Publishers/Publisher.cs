using AdvSearcher.Core.Entities.Publishers.ValueObjects;

namespace AdvSearcher.Core.Entities.Publishers;

public sealed class Publisher
{
    public PublisherId Id { get; init; }
    public PublisherData Data { get; init; }
    public bool IsIgnored { get; private set; }

    private Publisher() { } // EF core constructor

    public Publisher(PublisherData data) => Data = data;

    public void MakePublisherIgnored() => IsIgnored = true;
}
