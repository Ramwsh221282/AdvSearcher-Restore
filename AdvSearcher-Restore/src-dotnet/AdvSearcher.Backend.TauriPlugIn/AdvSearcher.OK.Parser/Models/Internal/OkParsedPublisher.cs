using AdvSearcher.Core.Entities.Publishers.Errors;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.OK.Parser.Models.Internal;

public sealed record OkParsedPublisher : IParsedPublisher
{
    public string Info { get; set; } = string.Empty;

    private OkParsedPublisher(string info) => Info = info;

    public static Result<IParsedPublisher> Create(Result<string> info) =>
        info.IsFailure ? PublisherErrors.InfoEmpty : new OkParsedPublisher(info);
}
