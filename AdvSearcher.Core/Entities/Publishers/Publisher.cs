namespace AdvSearcher.Core.Entities.Publishers;

public sealed class Publisher
{
    public PublisherId Id { get; init; }
    public PublisherData Data { get; init; }
    public bool IsIgnored { get; private set; }

    private Publisher() { } // ef core constructor

    public Publisher(PublisherData data)
    {
        Id = PublisherId.CreateEmpty();
        Data = data;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj is not Publisher publisher)
            return false;
        return publisher.Data == this.Data;
    }

    public void MakePublisherIgnored() => IsIgnored = true;

    public override int GetHashCode() => HashCode.Combine(Id, Data);
}
