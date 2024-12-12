using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Repository.InMemoryRepository;

namespace AdvSearcher.Core.Tests.ServiceUrlsTests;

[TestFixture]
[Category("ServiceUrlsCrudTests")]
public class ServiceUrlsCrudTests
{
    [Test, Order(1)]
    public async Task TestCreation()
    {
        var inMemoryRepository = new InMemoryServiceUrlRepository();
        var link = "http://localhost:5000";
        var url = ServiceUrlValue.Create(link);
        var mode = ServiceUrlMode.Loadable;
        await ServiceUrl.Create(url.Value, mode, inMemoryRepository.AddServiceUrlAsync);
        Assert.That(inMemoryRepository.Count, Is.EqualTo(1));
    }
}
