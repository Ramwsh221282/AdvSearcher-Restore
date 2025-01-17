using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Cian.Parser.Models.InternalModels;

public sealed record CianPublisher : IParsedPublisher
{
    public string Info { get; set; }

    private CianPublisher(string info) => Info = info;

    public static Result<IParsedPublisher> Create(string? info) =>
        string.IsNullOrWhiteSpace(info) ? ParserErrors.CantParsePublisher : new CianPublisher(info);
}
