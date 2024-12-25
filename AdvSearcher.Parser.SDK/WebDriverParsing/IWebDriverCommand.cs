namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public interface IWebDriverCommand<TCommandClassName>
{
    Task ExecuteAsync(WebDriverProvider provider);
}
