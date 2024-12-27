using System.Text;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal sealed record DomclickCookie(string Value, string Name, string Domain, string Path);

internal sealed class DomclickCookieFactory
{
    private readonly WebDriverProvider _provider;
    public string CookieHeaderValue { get; private set; } = string.Empty;

    public DomclickCookieFactory(WebDriverProvider provider) => _provider = provider;

    public DomclickCookie[] ExtractCookies()
    {
        if (_provider.Instance == null)
            return [];

        DomclickCookie[] cookies = _provider
            .Instance.Manage()
            .Cookies.AllCookies.Select(cookie => new DomclickCookie(
                cookie.Value,
                cookie.Name,
                cookie.Domain,
                cookie.Path
            ))
            .Where(cookie => cookie.Domain.Contains(".domclick.ru"))
            .ToArray();
        InstantiateCookieHeaderValue(cookies);
        return cookies;
    }

    private void InstantiateCookieHeaderValue(DomclickCookie[] cookies)
    {
        StringBuilder stringBuilder = new();
        foreach (var cookie in cookies)
        {
            stringBuilder.Append(CreateCookieForm(cookie));
        }
        CookieHeaderValue = stringBuilder.ToString().Trim();
    }

    private static string CreateCookieForm(DomclickCookie cookie)
    {
        string form = $"{cookie.Name}={cookie.Value}; ";
        return form;
    }
}
