using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.GetAllDivElements;

internal sealed class GetAllDivElementsQuery
    : IAvitoWebDriverQuery<GetAllDivElementsQuery, IEnumerable<IWebElement>>
{
    private const string TagName = "div";
    private readonly IWebElement _dialog;

    public GetAllDivElementsQuery(IWebElement dialog) => _dialog = dialog;

    public async Task<Result<IEnumerable<IWebElement>>> Execute(IWebDriver driver)
    {
        IReadOnlyCollection<IWebElement>? allDivElements = _dialog.FindElements(
            By.TagName(TagName)
        );
        return allDivElements == null
            ? new Error("Can't find all div elements")
            : Result<IEnumerable<IWebElement>>.Success(allDivElements);
    }
}
