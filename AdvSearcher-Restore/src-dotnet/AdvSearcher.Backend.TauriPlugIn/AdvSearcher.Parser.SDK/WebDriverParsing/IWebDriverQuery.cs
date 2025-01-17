namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public interface IWebDriverQuery<TResult>
{
    Task<TResult> ExecuteAsync(WebDriverProvider provider);
}
