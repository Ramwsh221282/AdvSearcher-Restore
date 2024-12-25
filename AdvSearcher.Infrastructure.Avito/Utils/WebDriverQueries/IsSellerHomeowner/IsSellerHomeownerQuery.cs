using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.IsSellerHomeowner;

internal sealed class IsSellerHomeownerQuery : IAvitoWebDriverQuery<IsSellerHomeownerQuery, bool>
{
    private readonly IEnumerable<IWebElement> _elements;
    private const string Constraint = "Частное лицо";

    public IsSellerHomeownerQuery(IEnumerable<IWebElement> elements) => _elements = elements;

    public async Task<Result<bool>> Execute(IWebDriver driver)
    {
        var elementWithHomeOwnerTag = _elements.FirstOrDefault(el =>
            el.Text.Contains(Constraint, StringComparison.OrdinalIgnoreCase)
        );
        if (elementWithHomeOwnerTag == null)
            return await Task.FromResult(true);
        return await Task.FromResult(false);
    }
}
