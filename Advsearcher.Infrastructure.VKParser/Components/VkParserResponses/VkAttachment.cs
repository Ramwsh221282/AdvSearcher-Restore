using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

internal sealed class VkAttachment(string url) : IParsedAttachment
{
    public string Url { get; } = url;

    public static Result<VkAttachment[]> TryExtractAttachments(JToken json)
    {
        var attachmentsToken = json["attachments"];
        if (attachmentsToken == null)
            return new Error("Нет вложений");
        var attachmentJsons = attachmentsToken.ToArray();
        var attachments = new List<VkAttachment>();
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
