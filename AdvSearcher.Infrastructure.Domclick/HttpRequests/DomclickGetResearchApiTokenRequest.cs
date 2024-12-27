using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickGetResearchApiTokenRequest : IHttpRequest
{
    private readonly RestRequest _request;
    public RestRequest Request => _request;

    public DomclickGetResearchApiTokenRequest(
        DomclickCookieFactory factory,
        DomclickFetchResult result
    )
    {
        string requestUrl = $"https://offer-card.domclick.ru/api/v3/public_request/{result.Id}";
        _request = new RestRequest(requestUrl)
            .AddHeader("Accept", "application/json, text/plan, */*")
            .AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
            .AddHeader("Connection", "keep-alive")
            .AddHeader("Cookie", factory.CookieHeaderValue)
            .AddHeader("Origin", "https://krasnoyarsk.domclick.ru")
            .AddHeader("Referer", "https://krasnoyarsk.domclick.ru/")
            .AddHeader("Sec-Fetch-Dest", "empty")
            .AddHeader("Sec-Fetch-Mode", "cors")
            .AddHeader("Sec-Fetch-Site", "same-site")
            .AddHeader(
                "sec-ch-ua",
                "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\""
            )
            .AddHeader("sec-ch-ua-mobile", "?0")
            .AddHeader("sec-ch-ua-platform", "\"Windows\"");
    }
}
