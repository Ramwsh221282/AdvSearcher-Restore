using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Domclick.Parser.InternalModels.DomclickParserResults;

public sealed record DomclickAttachment : IParsedAttachment
{
    public string Url { get; }

    private DomclickAttachment(string url) => Url = url;

    public static IParsedAttachment[] Create(DomclickFetchResult result)
    {
        List<IParsedAttachment> attachments = new();
        foreach (var photoUrl in result.PhotoUrls)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
                continue;
            attachments.Add(new DomclickAttachment(photoUrl));
        }
        return attachments.ToArray();
    }
}
