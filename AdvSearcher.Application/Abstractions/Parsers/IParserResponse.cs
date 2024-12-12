namespace AdvSearcher.Application.Abstractions.Parsers;

public interface IParserResponse
{
    IParsedAdvertisement Advertisement { get; }
    IParsedAttachment[] Attachments { get; }
    IParsedPublisher Publisher { get; }
}
