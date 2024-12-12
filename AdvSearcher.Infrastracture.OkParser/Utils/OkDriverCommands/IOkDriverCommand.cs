using OpenQA.Selenium;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands;

public interface IOkDriverCommand<TCommand>
    where TCommand : IOkDriverCommand<TCommand>
{
    Task ExecuteAsync(IWebDriver driver);
}
