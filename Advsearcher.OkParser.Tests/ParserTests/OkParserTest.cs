using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Infrastracture.OkParser.DependencyInjection;
using AdvSearcher.Parser.SDK.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Advsearcher.OkParser.Tests.ParserTests;

[TestFixture]
[Category("OkParser")]
public class OkParserTest
{
    private readonly IServiceCollection _services;

    public OkParserTest()
    {
        _services = new ServiceCollection();
        _services = _services.AddParserSDK().AddOkParser();
    }

    [Test, Order(1)]
    public async Task InvokeSinglePageTest()
    {
        var mode = ServiceUrlMode.Loadable;
        var urlValue = ServiceUrlValue.Create("https://ok.ru/group/70000006132389/topics");
        var url = ServiceUrl.Create(urlValue, mode);
        var provider = _services.BuildServiceProvider();
        var parser = provider.GetRequiredService<IParser<OkParserService>>();
        await parser.ParseData(url);
        var results = parser.Results;
        Assert.That(results, Is.Not.Empty);
    }
}
