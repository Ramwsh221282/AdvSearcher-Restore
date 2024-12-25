using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

namespace AdvSearcher.Parser.SDK.WebDriverParsing;

public sealed class WebDriverCommandsPipeline
{
    private readonly List<WebDriverCommand> _commands = [];

    public void AddCommand(WebDriverCommand command) => _commands.Add(command);

    public async Task ExecuteCommandsAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;

        foreach (var command in _commands)
        {
            await command.ExecuteAsync(provider);
        }
    }
}
