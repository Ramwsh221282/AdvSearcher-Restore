using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Infrastructure.Avito.DependencyInjection;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.AvitoParser.ConsoleTests
{
    class Program
    {
        static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection = serviceCollection.AddParserSDK();
            AvitoParsingService diModifier = new AvitoParsingService();
            serviceCollection = diModifier.ModifyServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            Invoke(serviceProvider.GetRequiredService<IParser>()).Wait();
        }

        static async Task Invoke(IParser parser)
        {
            ServiceUrlMode mode = ServiceUrlMode.Loadable;
            ServiceUrlValue value = ServiceUrlValue.Create(
                "https://www.avito.ru/lesosibirsk/kvartiry/prodam-ASgBAgICAUSSA8YQ?context=H4sIAAAAAAAA_wEfAOD_YToxOntzOjg6ImZyb21QYWdlIjtzOjM6Im1hcCI7fRd7hMQfAAAA&s=104"
            );
            ServiceUrl url = ServiceUrl.Create(value, mode);
            await parser.ParseData(url);
            IReadOnlyCollection<IParserResponse> responses = parser.Results;
            int bpoint = 0;
        }
    }
}
