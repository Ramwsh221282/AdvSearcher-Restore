using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.IsSellerHomeowner;

internal sealed class IsSellerHomeownerQuery : IWebDriverQuery<IsSellerHomeownerQuery, bool>
{
    private readonly IEnumerable<IWebElement> _elements;
    private const string Constraint = "Частное лицо";

    public IsSellerHomeownerQuery(IEnumerable<IWebElement> elements) => _elements = elements;

    public async Task<bool> ExecuteAsync(WebDriverProvider provider)
    {
        var elementWithHomeOwnerTag = _elements.FirstOrDefault(el =>
            el.Text.Contains(Constraint, StringComparison.OrdinalIgnoreCase)
        );
        if (elementWithHomeOwnerTag == null)
            return await Task.FromResult(true);
        return await Task.FromResult(false);
    }
}
