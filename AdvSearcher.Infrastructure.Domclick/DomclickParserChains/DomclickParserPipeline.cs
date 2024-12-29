using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal sealed class DomclickParserPipeline
{
    public List<DomclickFetchResult> FetchResults { get; init; } = [];
    private readonly List<IParserResponse> _responses = [];
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    public void AddFetchResult(DomclickFetchResult result)
    {
        if (!result.IsAgent)
            FetchResults.Add(result);
    }

    public void AddParserResponse(IParserResponse response) => _responses.Add(response);
}
