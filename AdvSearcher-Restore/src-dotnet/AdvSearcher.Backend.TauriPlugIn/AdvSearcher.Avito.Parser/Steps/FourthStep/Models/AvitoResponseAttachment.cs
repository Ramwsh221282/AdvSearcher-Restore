using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Models;

public sealed record AvitoResponseAttachment : IParsedAttachment
{
    public string Url { get; }

    private AvitoResponseAttachment(string url) => Url = url;

    public static IParsedAttachment[] Create(AvitoAdvertisement advertisement)
    {
        List<IParsedAttachment> attachments = [];
        foreach (var url in advertisement.Photos)
        {
            if (string.IsNullOrWhiteSpace(url))
                continue;
            AvitoResponseAttachment attachment = new AvitoResponseAttachment(url);
            attachments.Add(attachment);
        }
        return attachments.ToArray();
    }
}
