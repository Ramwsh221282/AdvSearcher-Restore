namespace AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

public abstract class WebDriverCommand
{
    public abstract Task ExecuteAsync(WebDriverProvider provider);
}
