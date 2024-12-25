namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public interface IWebDriverQuery<TWebDriverQueryClass, TQueryResult>
{
    Task<TQueryResult> ExecuteAsync(WebDriverProvider provider);
}
