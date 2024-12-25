using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;

namespace AdvSearcher.Infrastructure.Domclick;

internal sealed class DomclickParser(
    IDomclickRequestExecutor executor,
    IDomclickFetchingResultFactory factory,
    DomclickAdvertisementsFactory advertisementsFactory
) : IParser<DomclickParserService>
{
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task<Result<bool>> ParseData(ServiceUrl url = null!)
    {
        DomclickPageRequestSender sender = new DomclickPageRequestSender(executor, factory);
        await sender.ConstructFetchResults();
        await advertisementsFactory.Construct(sender.Results);
        _results.AddRange(advertisementsFactory.Results);
        return true;
    }
}
