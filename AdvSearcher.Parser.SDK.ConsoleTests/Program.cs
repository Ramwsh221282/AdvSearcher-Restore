using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
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
            TestCian(parserProvider).Wait();
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
            int bpoint = 0;
        }
    }
}
