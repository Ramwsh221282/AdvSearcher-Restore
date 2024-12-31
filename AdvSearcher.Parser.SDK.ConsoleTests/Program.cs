using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Parser.SDK.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK.ConsoleTests
{
    class Program
    {
        static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            services = services.AddParserSDK();
            IServiceProvider provider = services.BuildServiceProvider();
            ParserProvider parserProvider = provider.GetRequiredService<ParserProvider>();
            //TestCian(parserProvider).Wait();
            //TestAvito(parserProvider).Wait();
            //TestOk(parserProvider).Wait();
            //TestFileInteractions(provider).Wait();
            //TestVk(parserProvider, provider).Wait();
            //TestFastAvito(parserProvider).Wait();
            TestDomclick(parserProvider).Wait();
        }

        static async Task TestDomclick(ParserProvider provider)
        {
            IParser parser = provider.GetParser("DomclickParser");
            await parser.ParseData(null!);
            IReadOnlyCollection<IParserResponse> response = parser.Results;
            Console.WriteLine($"Domclick processed. Results: {response.Count}");
        }

        static async Task TestFastAvito(ParserProvider parserProvider)
        {
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrlValue value = ServiceUrlValue.Create(
                "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104"
            );
            ServiceUrl url = ServiceUrl.Create(value, mode);
            IParser parser = parserProvider.GetParser("AvitoFastParser");
            await parser.ParseData(url);
            IReadOnlyCollection<IParserResponse> results = parser.Results;
            Console.WriteLine($"Fast Avito Results: {results.Count}");
        }

        static async Task TestVk(ParserProvider parserProvider, IServiceProvider serviceProvider)
        {
            Option serviceAccessToken = new Option(
                "ServiceToken",
                "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95"
            );
            Option oAuthAccessToken = new Option(
                "OAuthToken",
                "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ"
            );
            IOptionManager optionManager = serviceProvider.GetRequiredService<IOptionManager>();
            IOptionProcessor writer = optionManager.CreateWrite("VK_TOKENS.txt");
            await writer.Process(serviceAccessToken);
            await writer.Process(oAuthAccessToken);
            int bpoint = 0;
            IParser parser = parserProvider.GetParser("VkParser");
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrlValue value = ServiceUrlValue.Create("https://vk.com/les_gorod");
            ServiceUrl url = ServiceUrl.Create(value, mode);
            await parser.ParseData(url);
            IReadOnlyCollection<IParserResponse> responses = parser.Results;
            Console.WriteLine($"Responses results VK: {responses.Count}");
            IOptionProcessor flusher = optionManager.CreateFlusher("VK_TOKENS.txt");
            await flusher.Process(serviceAccessToken);
        }

        static async Task TestFileInteractions(IServiceProvider provider)
        {
            IOptionManager optionManager = provider.GetRequiredService<IOptionManager>();
            string fileName = "VK_TOKENS.txt";
            IOptionProcessor writer = optionManager.CreateWrite(fileName);
            string ServiceAccessToken =
                "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95";
            Option SAK = new Option(nameof(ServiceAccessToken), ServiceAccessToken);
            await writer.Process(SAK);

            string OAuthAccessToken =
                "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ";
            Option OAK = new Option(nameof(OAuthAccessToken), OAuthAccessToken);
            await writer.Process(OAK);

            string ApiVesrion = "5.131";
            Option APIV = new Option(nameof(ApiVesrion), ApiVesrion);
            await writer.Process(APIV);

            IOptionProcessor reader = optionManager.CreateReader(fileName);
            SAK = await reader.Process(SAK);
            OAK = await reader.Process(OAK);
            APIV = await reader.Process(APIV);

            IOptionProcessor flusher = optionManager.CreateFlusher(fileName);
            await flusher.Process(null!);
        }

        static async Task TestCian(ParserProvider provider)
        {
            IParser cianParser = provider.GetParser("CianParser");
            ServiceUrlValue value = ServiceUrlValue.Create(
                "https://krasnoyarsk.cian.ru/kupit-kvartiru-krasnoyarskiy-kray-lesosibirsk/"
            );
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrl url = ServiceUrl.Create(value, mode);
            await cianParser.ParseData(url);
            IReadOnlyCollection<IParserResponse> responses = cianParser.Results;
            Console.WriteLine($"Cian passed. Cian responses: {responses.Count}");
            int bpoint = 0;
        }

        static async Task TestAvito(ParserProvider provider)
        {
            IParser avitoParser = provider.GetParser("AvitoParser");
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrlValue value = ServiceUrlValue.Create(
                "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104"
            );
            ServiceUrl url = ServiceUrl.Create(value, mode);
            await avitoParser.ParseData(url);
            IReadOnlyCollection<IParserResponse> responses = avitoParser.Results;
            int bpoint = 0;
        }

        static async Task TestOk(ParserProvider provider)
        {
            IParser okParser = provider.GetParser("OkParser");
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrlValue value = ServiceUrlValue.Create(
                "https://ok.ru/group/70000006132389/topics"
            );
            ServiceUrl url = ServiceUrl.Create(value, mode);
            await okParser.ParseData(url);
            IReadOnlyCollection<IParserResponse> responses = okParser.Results;
            Console.WriteLine($"Ok passed. Ok responses: {responses.Count}");
            int bpoint = 0;
        }
    }
}
