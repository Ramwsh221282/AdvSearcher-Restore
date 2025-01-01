using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.ConsoleTests.ConsoleTestsVk;

public sealed class VkTests
{
    private readonly IServiceCollection _services;

    public VkTests(IServiceCollection serviceCollection) => _services = serviceCollection;

    private ParserProvider GetParserProvider()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        return provider.GetRequiredService<ParserProvider>();
    }

    private IOptionManager GetOptionsManager()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        return provider.GetRequiredService<IOptionManager>();
    }

    public async Task TestVkSingle()
    {
        Option serviceAccessToken = new Option(
            "ServiceToken",
            "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95"
        );
        Option oAuthAccessToken = new Option(
            "OAuthToken",
            "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ"
        );
        IOptionManager optionsManager = GetOptionsManager();
        ParserProvider parserProvider = GetParserProvider();
        IOptionProcessor writer = optionsManager.CreateWrite("VK_TOKENS.txt");
        await writer.Process(serviceAccessToken);
        await writer.Process(oAuthAccessToken);
        IParser parser = parserProvider.GetParser("VkParser");
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create("https://vk.com/les_gorod");
        ServiceUrlService service = ServiceUrlService.Create("Vk");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        await parser.ParseData(url);
        IReadOnlyCollection<IParserResponse> responses = parser.Results;
        Console.WriteLine($"Responses results VK: {responses.Count}");
        IOptionProcessor flusher = optionsManager.CreateFlusher("VK_TOKENS.txt");
        await flusher.Process(serviceAccessToken);
    }

    public async Task TestVkDateFilter()
    {
        Option serviceAccessToken = new Option(
            "ServiceToken",
            "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95"
        );
        Option oAuthAccessToken = new Option(
            "OAuthToken",
            "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ"
        );
        IOptionManager optionsManager = GetOptionsManager();
        ParserProvider parserProvider = GetParserProvider();
        IOptionProcessor writer = optionsManager.CreateWrite("VK_TOKENS.txt");
        await writer.Process(serviceAccessToken);
        await writer.Process(oAuthAccessToken);
        IParser parser = parserProvider.GetParser("VkParser");
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create("https://vk.com/les_gorod");
        ServiceUrlService service = ServiceUrlService.Create("Vk");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        DateOnly startDate = new DateOnly(2024, 12, 20);
        DateOnly endDate = new DateOnly(2024, 12, 30);
        List<ParserFilterOption> options = ParserFilterExtensions.CreateOptionsList(
            startDate,
            endDate
        );
        await parser.ParseData(url, options);
        IReadOnlyCollection<IParserResponse> responses = parser.Results;
        foreach (var response in responses)
        {
            Console.WriteLine($"Advertisement publisher: {response.Publisher.Info}");
            Console.WriteLine($"Advertisement date: {response.Advertisement.Date}");
            Console.WriteLine($"Advertisement id: {response.Advertisement.Id}");
            Console.WriteLine($"Advertisement service: {response.ServiceName}");
        }
        Console.WriteLine($"Responses results VK: {responses.Count}");
        IOptionProcessor flusher = optionsManager.CreateFlusher("VK_TOKENS.txt");
        await flusher.Process(serviceAccessToken);
    }

    public async Task TestVkDateFilterAndNameFilter()
    {
        PublisherData data = PublisherData.Create("Наталья Иванова");
        Publisher publisher = new Publisher(data);
        publisher.MakePublisherIgnored();
        Option serviceAccessToken = new Option(
            "ServiceToken",
            "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95"
        );
        Option oAuthAccessToken = new Option(
            "OAuthToken",
            "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ"
        );
        IOptionManager optionsManager = GetOptionsManager();
        ParserProvider parserProvider = GetParserProvider();
        IOptionProcessor writer = optionsManager.CreateWrite("VK_TOKENS.txt");
        await writer.Process(serviceAccessToken);
        await writer.Process(oAuthAccessToken);
        IParser parser = parserProvider.GetParser("VkParser");
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create("https://vk.com/les_gorod");
        ServiceUrlService service = ServiceUrlService.Create("Vk");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        DateOnly startDate = new DateOnly(2024, 12, 20);
        DateOnly endDate = new DateOnly(2024, 12, 30);
        List<ParserFilterOption> options = ParserFilterExtensions.CreateOptionsList(
            startDate,
            endDate,
            [publisher]
        );
        await parser.ParseData(url, options);
        IReadOnlyCollection<IParserResponse> responses = parser.Results;
        foreach (var response in responses)
        {
            Console.WriteLine($"Advertisement date: {response.Advertisement.Date}");
            Console.WriteLine($"Advertisement publisher: {response.Publisher.Info}");
            Console.WriteLine($"Advertisement id: {response.Advertisement.Id}");
            Console.WriteLine($"Advertisement service: {response.ServiceName}");
        }
        Console.WriteLine($"Responses results VK: {responses.Count}");
        IOptionProcessor flusher = optionsManager.CreateFlusher("VK_TOKENS.txt");
        await flusher.Process(serviceAccessToken);
    }

    public async Task TestWithDateFilterNameFilterCacheFilter()
    {
        PublisherData data = PublisherData.Create("Наталья Иванова");
        Publisher publisher = new Publisher(data);
        publisher.MakePublisherIgnored();
        Option serviceAccessToken = new Option(
            "ServiceToken",
            "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95"
        );
        Option oAuthAccessToken = new Option(
            "OAuthToken",
            "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ"
        );
        IOptionManager optionsManager = GetOptionsManager();
        ParserProvider parserProvider = GetParserProvider();
        IOptionProcessor writer = optionsManager.CreateWrite("VK_TOKENS.txt");
        await writer.Process(serviceAccessToken);
        await writer.Process(oAuthAccessToken);
        IParser parser = parserProvider.GetParser("VkParser");
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        ServiceUrlValue value = ServiceUrlValue.Create("https://vk.com/les_gorod");
        ServiceUrlService service = ServiceUrlService.Create("Vk");
        ServiceUrl url = new ServiceUrl(value, mode, service);
        DateOnly startDate = new DateOnly(2024, 12, 20);
        DateOnly endDate = new DateOnly(2024, 12, 30);
        List<ParserFilterOption> options = ParserFilterExtensions.CreateOptionsList(
            startDate,
            endDate,
            [publisher]
        );
        await parser.ParseData(url, options);
        IReadOnlyCollection<IParserResponse> responses = parser.Results;
        List<Advertisement> ads = responses.ToAdvertisements().ToList();
        List<CachedAdvertisement> cached = ads.Select(ad => ad.ToCachedAdvertisement()).ToList();
        parserProvider = GetParserProvider();
        parser = parserProvider.GetParser("VkParser");
        options = ParserFilterExtensions.CreateOptionsList(startDate, endDate, [publisher], cached);
        await parser.ParseData(url, options);
        responses = parser.Results;
        Console.WriteLine($"Responses results VK: {responses.Count}");
        foreach (var response in responses)
        {
            Console.WriteLine($"Advertisement date: {response.Advertisement.Date}");
            Console.WriteLine($"Advertisement publisher: {response.Publisher.Info}");
            Console.WriteLine($"Advertisement id: {response.Advertisement.Id}");
            Console.WriteLine($"Advertisement service: {response.ServiceName}");
        }
        IOptionProcessor flusher = optionsManager.CreateFlusher("VK_TOKENS.txt");
        await flusher.Process(serviceAccessToken);
    }
}
