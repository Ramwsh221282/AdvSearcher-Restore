using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal interface IDomclickRequest
{
    public RestRequest Request { get; }
}
