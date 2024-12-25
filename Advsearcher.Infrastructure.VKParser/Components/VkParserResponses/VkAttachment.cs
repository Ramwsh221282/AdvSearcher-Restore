using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

internal sealed class VkAttachment(string url) : IParsedAttachment
{
    public string Url { get; } = url;

    public static Result<IParsedAttachment[]> TryExtractAttachments(JToken json)
    {
        List<IParsedAttachment> attachments = [];
        var attachmentsToken = json["attachments"];
        if (attachmentsToken == null)
            return new Error("Нет вложений");
        var attachmentJsons = attachmentsToken.ToArray();
        foreach (var attachment in attachmentJsons)
        {
            var typeToken = attachment["type"];
            if (typeToken == null)
                continue;
            if (typeToken.ToString() != "photo")
                continue;
            var sizesToken = attachment["photo"]!["sizes"];
            if (sizesToken == null)
                continue;
            var items = sizesToken.ToArray();
            var url = items[^1]["url"]!.ToString();
            attachments.Add(new VkAttachment(url));
        }

        return attachments.ToArray();
    }
}
