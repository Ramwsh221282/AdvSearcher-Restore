using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
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
    public void TestDateConversion()
    {
        var provider = _services.BuildServiceProvider();
        IAdvertisementDateConverter<AvitoParserService> dateConverter = provider.GetRequiredService<
            IAdvertisementDateConverter<AvitoParserService>
        >();

        List<(Result<DateOnly>, string)> dates = [];
        List<string> stringDates = [];
        stringDates.Add("7 дней назад");
        stringDates.Add("3 дня назад");
        stringDates.Add("1 неделю назад");
        stringDates.Add("2 недели назад");
        stringDates.Add("3 недели назад");
        stringDates.Add("25 ноября");
        stringDates.Add("23 ноября");
        stringDates.Add("8 ноября");

        foreach (var dateString in stringDates)
        {
            dates.Add((dateConverter.Convert(dateString), dateString));
        }
        Assert.That(dates.Select(d => d.Item1).Where(d => d.IsFailure).Count(), Is.EqualTo(0));
    }

    [Test, Order(2)]
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
        Assert.That(parser.Results, Is.Not.Empty);
    }
}
