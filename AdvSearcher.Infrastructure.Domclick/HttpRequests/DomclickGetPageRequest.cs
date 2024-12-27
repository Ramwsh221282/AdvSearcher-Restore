using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickGetPageRequest : IHttpRequest
{
    private readonly RestRequest _request;

    public RestRequest Request => _request;

    public DomclickGetPageRequest(string qraturValue, int offset = 20)
    {
        _request = new RestRequest();
        string url =
            $"https://bff-search-web.domclick.ru/api/offers/v1?address=6b2a4aad-bd39-4982-9ee0-2cc25449964b&offset={offset}&limit=20&sort=qi&sort_dir=desc&deal_type=sale&category=living&offer_type=flat&offer_type=layout&aids=650885&sort_by_tariff_date=1";
        this._request = new RestRequest(url);
        this._request.AddHeader("Accept", "application/json, text/plain, */*");
        this._request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        this._request.AddHeader("Connection", "keep-alive");
        this._request.AddHeader("Origin", "https://krasnoyarsk.domclick.ru");
        this._request.AddHeader("Referer", "https://krasnoyarsk.domclick.ru/");
        this._request.AddHeader("Sec-Fetch-Dest", "empty");
        this._request.AddHeader("Sec-Fetch-Mode", "cors");
        this._request.AddHeader("Sec-Fetch-Site", "same-site");
        this._request.AddHeader(
            "sec-ch-ua",
            "\"Chromium\";v=\"128\", \"Not;A=Brand\";v=\"24\", \"Google Chrome\";v=\"128\""
        );
        this._request.AddHeader("sec-ch-ua-mobile", "?0");
        this._request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
        this._request.AddHeader("Cookie", qraturValue);
    }
}
