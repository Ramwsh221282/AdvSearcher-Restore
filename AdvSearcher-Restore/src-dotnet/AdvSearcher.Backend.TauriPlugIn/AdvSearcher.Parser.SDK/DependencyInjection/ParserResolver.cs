using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.DependencyInjection;

public sealed class ParserResolver(IServiceScopeFactory factory)
{
    private readonly IServiceScopeFactory _factory = factory;

    public IParser Resolve(string parserName)
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IEnumerable<IParser> parsers = provider.GetServices<IParser>();
        IParser? parser = parsers.FirstOrDefault(p => p.GetType().Name == parserName);
        if (parser == null)
            throw new NotSupportedException();
        return parser;
    }
}
