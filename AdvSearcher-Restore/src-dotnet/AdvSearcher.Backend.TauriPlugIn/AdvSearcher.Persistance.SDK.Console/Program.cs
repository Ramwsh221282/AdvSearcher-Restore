using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK.Console
{
    class Program
    {
        static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddPersistanceSDK();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IServiceUrlRepository serviceUrlRepository =
                serviceProvider.GetRequiredService<IServiceUrlRepository>();
            serviceUrlRepository
                .Get(ServiceUrlMode.Loadable, ServiceUrlService.Create("VK"))
                .Wait();
        }
    }
}
