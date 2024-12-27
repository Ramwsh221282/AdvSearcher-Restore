using System.Text;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.Primitives;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal abstract record DomclickQratorCookies(string Name, string Value);

internal record DomclickQratorSsid2(string Name, string Value) : DomclickQratorCookies(Name, Value);

internal record DomclickQratorJsid2(string Name, string Value) : DomclickQratorCookies(Name, Value);

internal sealed class DomclickQratorSsid2Factory
{
    private readonly DomclickCookie[] _cookies;

    public DomclickQratorSsid2Factory(WebDriverProvider provider)
    {
        _cookies = provider
            .Instance!.Manage()
            .Cookies.AllCookies.Select(cookie => new DomclickCookie(
                cookie.Value,
                cookie.Name,
                cookie.Domain,
                cookie.Path
            ))
            .Where(cookie => cookie.Domain.Contains(".domclick.ru"))
            .ToArray();
    }

    public DomclickQratorCookies? CreateSsid2()
    {
        DomclickCookie? cookieWithQrator = _cookies.FirstOrDefault(cookie =>
            cookie.Name == "qrator_ssid2"
        );
        if (cookieWithQrator == null)
            return null;
        return new DomclickQratorSsid2(cookieWithQrator.Name, cookieWithQrator.Value);
    }

    public DomclickQratorCookies? CreateJsid2()
    {
        DomclickCookie? cookieWithQrator = _cookies.FirstOrDefault(cookie =>
            cookie.Name == "qrator_jsid2"
        );
        if (cookieWithQrator == null)
            return null;
        return new DomclickQratorJsid2(cookieWithQrator.Name, cookieWithQrator.Value);
    }

    public string CreateValues(params DomclickQratorCookies[] cookies)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var cookie in cookies)
        {
            sb.Append($"{cookie.Name}={cookie.Value}; ");
        }
        return sb.ToString().Trim();
    }

    public string CreateValue(DomclickQratorCookies? cookie)
    {
        if (cookie == null)
            return string.Empty;
        return ExtractCookieForm(cookie);
    }

    private string ExtractCookieForm(DomclickQratorCookies? cookie)
    {
        if (cookie == null)
            return string.Empty;
        return $"{cookie.Name}={cookie.Value};";
    }
}
