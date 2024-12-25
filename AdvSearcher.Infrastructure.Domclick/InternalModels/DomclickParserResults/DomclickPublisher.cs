using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;

internal sealed record DomclickPublisher : IParsedPublisher
{
    public string Info { get; set; }

    private DomclickPublisher(string info) => Info = info;

    public static Result<IParsedPublisher> Create(DomclickFetchResult result)
    {
        if (string.IsNullOrWhiteSpace(result.PhoneNumber))
            return ParserErrors.CantParsePublisher;
        return new DomclickPublisher(result.PhoneNumber);
    }
}
