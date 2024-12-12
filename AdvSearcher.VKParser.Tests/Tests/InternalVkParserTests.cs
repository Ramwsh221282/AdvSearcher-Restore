using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using Advsearcher.Infrastructure.VKParser;
using Advsearcher.Infrastructure.VKParser.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.VKParser.Tests.Tests;

[TestFixture]
[Category("InternalVkTests")]
public class InternalVkParserTests
{
    private readonly IServiceCollection _service;

    public InternalVkParserTests()
    {
        _service = new ServiceCollection();
        _service = _service.AddVkParser();
    }

    [Test, Order(1)]
    public async Task TestVkParsingOneUrl()
    {
        var provider = _service.BuildServiceProvider();
        var parser = provider.GetRequiredService<IParser<VkParser>>();
        var mode = ServiceUrlMode.Loadable;
        var urlValue = ServiceUrlValue.Create("https://vk.com/les_gorod");
        var url = ServiceUrl.Create(urlValue, mode);
        await parser.ParseData(url);
        var data = parser.Results;
        Assert.That(data, Is.Not.Empty);
    }
}
