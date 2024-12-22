using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Infrastructure.Avito.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.AvitoParser.Tests.Tests;

[TestFixture]
[Category("AvitoParser")]
public sealed class AvitoParsingTests
{
    private readonly IServiceCollection _services;

    public AvitoParsingTests()
    {
        _services = new ServiceCollection();
        _services.AddAvitoParsingServices();
    }

    [Test, Order(1)]
    public async Task InvokeSinglePageParsing()
    {
        const string pageUrl =
            "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104";
        var mode = ServiceUrlMode.Loadable;
        var urlValue = ServiceUrlValue.Create(pageUrl);
        var url = ServiceUrl.Create(urlValue, mode);
        var provider = _services.BuildServiceProvider();
        var parser = provider.GetRequiredService<IParser<AvitoParserService>>();
        await parser.ParseData(url);
        var flag = true;
        Assert.That(flag, Is.True);
    }
}
