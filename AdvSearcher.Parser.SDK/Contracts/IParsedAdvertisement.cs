namespace AdvSearcher.Parser.SDK.Contracts;

public interface IParsedAdvertisement
{
    string Id { get; }
    string Url { get; }
    string Content { get; }
    DateOnly Date { get; }
}
