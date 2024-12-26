using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetAllDivElements;

internal sealed class GetAllDivElementsQuery
    : IWebDriverQuery<GetAllDivElementsQuery, Result<IEnumerable<IWebElement>>>
{
    private const string TagName = "div";
    private readonly IWebElement _dialog;

    public GetAllDivElementsQuery(IWebElement dialog) => _dialog = dialog;

    public async Task<Result<IEnumerable<IWebElement>>> ExecuteAsync(WebDriverProvider provider)
    {
        IReadOnlyCollection<IWebElement>? allDivElements = _dialog.FindElements(
            By.TagName(TagName)
        );
        return allDivElements == null
            ? new Error("Can't find all div elements")
            : Result<IEnumerable<IWebElement>>.Success(allDivElements);
    }
}
