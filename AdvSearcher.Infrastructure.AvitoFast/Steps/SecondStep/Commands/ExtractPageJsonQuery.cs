using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep.Commands;

internal sealed class ExtractPageJsonQuery : IWebDriverQuery<ExtractPageJsonQuery, Result<string>>
{
    public async Task<Result<string>> ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return new Error("Driver provider instance was null");

        IWebElement? formatter = FindFormatterElement(provider);
        if (formatter == null)
            return new Error("No formatter found");
        formatter.Click();

        IWebElement? preElement = FindPreElement(provider);
        return preElement switch
        {
            null => await Task.FromResult(new Error("Can't find data container element")),
            _ when string.IsNullOrWhiteSpace(preElement.Text) => await Task.FromResult(
                new Error("Can't get data")
            ),
            _ => await Task.FromResult(preElement.Text),
        };
    }

    private IWebElement? FindPreElement(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return null;
        IWebElement? preElement = null;
        while (preElement == null)
        {
            try
            {
                preElement = provider.Instance.FindElement(By.TagName("pre"));
            }
            catch
            {
                // ignored
            }
        }
        return preElement;
    }

    private IWebElement? FindFormatterElement(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return null;
        IWebElement? formatter = null;
        while (formatter == null)
        {
            try
            {
                formatter = provider.Instance.FindElement(By.ClassName("json-formatter-container"));
            }
            catch
            {
                // ignored
            }
        }
        return formatter;
    }
}
