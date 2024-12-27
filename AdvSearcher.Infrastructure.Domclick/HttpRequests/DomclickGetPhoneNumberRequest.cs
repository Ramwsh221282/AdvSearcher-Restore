using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickGetPhoneNumberRequest : IHttpRequest
{
    private readonly RestRequest _request;
    public RestRequest Request => _request;

    public DomclickGetPhoneNumberRequest(
        DomclickResearchApiToken token,
        DomclickCookieFactory factory,
        DomclickFetchResult item
    )
    {
        string url = $"https://offer-card.domclick.ru/api/v3/offers/phone/{item.Id}";
        _request = new RestRequest(url)
            .AddHeader("Accept", "application/json, text/plain, */*")
            .AddHeader("Accept-Encoding", "gzip, deflate, br, zstd")
            .AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
            .AddHeader("Connection", "keep-alive")
            .AddHeader("Cookie", factory.CookieHeaderValue)
            .AddHeader("Host", "offer-card.domclick.ru")
            .AddHeader("Origin", "https://krasnoyarsk.domclick.ru")
            .AddHeader("Referer", "https://krasnoyarsk.domclick.ru/")
            .AddHeader("research-api-token", token.Token)
            .AddHeader(
                "Sec-Ch-Ua",
                "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\""
            )
            .AddHeader("Sec-Ch-Ua-Mobile", "?0")
            .AddHeader("Sec-Ch-Ua-Platform", "\"Windows\"")
            .AddHeader("Sec-Fetch-Dest", "empty")
            .AddHeader("Sec-Fetch-Mode", "cors")
            .AddHeader("Sec-Fetch-Site", "same-site")
            .AddHeader(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36"
            );
    }
}
