using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.ConsoleTests.ConsoleAvitoTests;

public class AvitoTests
{
    private readonly IServiceCollection _services;

    public AvitoTests(IServiceCollection services) => _services = services;

    private ParserProvider GetProvider()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        return provider.GetRequiredService<ParserProvider>();
    }

    public async Task TestAvitoFast()
    {
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create(
            "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104"
        );
        ServiceUrlService service = ServiceUrlService.Create("Avito");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        IParser parser = GetProvider().GetParser("AvitoFastParser");
        await parser.ParseData(url);
        IReadOnlyCollection<IParserResponse> results = parser.Results;
        Console.WriteLine($"Fast Avito Results: {results.Count}");
    }

    public async Task TestAvitoFastDateFilter()
    {
        DateOnly startDate = new DateOnly(2024, 12, 20);
        DateOnly endDate = new DateOnly(2024, 12, 30);
        List<ParserFilterOption> options = ParserFilterExtensions.CreateOptionsList(
            startDate,
            endDate
        );
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create(
            "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104"
        );
        ServiceUrlService service = ServiceUrlService.Create("Avito");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        IParser parser = GetProvider().GetParser("AvitoFastParser");
        await parser.ParseData(url, options);
        IReadOnlyCollection<IParserResponse> results = parser.Results;
        Console.WriteLine($"Fast Avito Results: {results.Count}");
        foreach (var result in results)
        {
            Console.WriteLine(result.Advertisement.Date);
        }
    }
}
