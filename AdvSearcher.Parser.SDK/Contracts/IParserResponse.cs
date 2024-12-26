namespace AdvSearcher.Parser.SDK.Contracts;

public interface IParserResponse
{
    IParsedAdvertisement Advertisement { get; }
    IParsedAttachment[] Attachments { get; }
    IParsedPublisher Publisher { get; }
}
