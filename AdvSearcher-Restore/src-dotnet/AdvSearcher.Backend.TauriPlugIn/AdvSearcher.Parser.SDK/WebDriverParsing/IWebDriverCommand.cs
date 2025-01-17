namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public interface IWebDriverCommand
{
    Task ExecuteAsync(WebDriverProvider provider);
}
