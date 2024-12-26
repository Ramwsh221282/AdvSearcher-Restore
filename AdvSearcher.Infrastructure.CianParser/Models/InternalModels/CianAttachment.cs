using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.CianParser.Models.InternalModels;

internal sealed record CianAttachment : IParsedAttachment
{
    public string Url { get; }

    private CianAttachment(string url) => Url = url;

    public static Result<IParsedAttachment> Create(string? url) =>
        string.IsNullOrWhiteSpace(url)
            ? ParserErrors.CantParseAttachmentUrl
            : new CianAttachment(url);
}
