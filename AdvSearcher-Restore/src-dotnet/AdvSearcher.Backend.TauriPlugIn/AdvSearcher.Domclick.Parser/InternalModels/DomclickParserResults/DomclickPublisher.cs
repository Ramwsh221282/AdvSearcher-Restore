using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Domclick.Parser.InternalModels.DomclickParserResults;

public sealed record DomclickPublisher : IParsedPublisher
{
    public string Info { get; set; }

    private DomclickPublisher(string info) => Info = info;

    public static Result<IParsedPublisher> Create(DomclickFetchResult result)
    {
        if (string.IsNullOrWhiteSpace(result.PhoneNumber))
            return ParserErrors.CantParsePublisher;
        StringBuilder stringBuilder = new StringBuilder()
            .AppendLine(result.FullName)
            .AppendLine(result.PhoneNumber);
        return new DomclickPublisher(stringBuilder.ToString());
    }
}
