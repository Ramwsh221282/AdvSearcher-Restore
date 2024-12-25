using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Infrastructure.Domclick.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Domclick.Tests;

[TestFixture]
[Category("Domclick")]
public class DomclickTests
{
    private readonly IServiceCollection _services;

    public DomclickTests()
    {
        _services = new ServiceCollection();
        _services = _services.AddDomclickParsingServices();
    }

    [Test, Order(1)]
    public async Task TestDomclick()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        IParser<DomclickParserService> parser = provider.GetRequiredService<
            IParser<DomclickParserService>
        >();
        await parser.ParseData(null!);
        IReadOnlyCollection<IParserResponse> responses = parser.Results;
        Assert.That(responses, Is.Not.Empty);
    }
}
