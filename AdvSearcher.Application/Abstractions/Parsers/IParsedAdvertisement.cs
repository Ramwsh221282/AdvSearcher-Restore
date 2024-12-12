namespace AdvSearcher.Application.Abstractions.Parsers;

public interface IParsedAdvertisement
{
    string Id { get; }
    string Url { get; }
    string Content { get; }
    DateOnly Date { get; }
}
