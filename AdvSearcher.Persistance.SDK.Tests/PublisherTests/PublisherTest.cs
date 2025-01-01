using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK.Tests.PublisherTests;

[TestFixture]
[Category("Publisher")]
public sealed class PublisherTest
{
    private readonly IServiceCollection _services;

    public PublisherTest()
    {
        _services = new ServiceCollection();
        _services = _services.AddPersistanceSDK();
    }

    [Test, Order(1)]
    public async Task CreateAndDeletePublisher()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        PublisherData data = PublisherData.Create("Some publisher data");
        Publisher publisher = new Publisher(data);
        IPublishersRepository repository = provider.GetRequiredService<IPublishersRepository>();
        Result<RepositoryOperationResult> result = await repository.Add(publisher);
        result = await repository.Remove(publisher);
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test, Order(2)]
    public async Task CreateTwoPublishers_Id_AutoIncrementTest()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        IPublishersRepository repository = provider.GetRequiredService<IPublishersRepository>();
        PublisherData dataFirst = PublisherData.Create("First publisher data");
        PublisherData dataSecond = PublisherData.Create("Second publisher data");
        Publisher publisher1 = new Publisher(dataFirst);
        Publisher publisher2 = new Publisher(dataSecond);
        Result<RepositoryOperationResult> result = await repository.Add(publisher1);
        result = await repository.Add(publisher2);
        List<RepositoryOperationResult> results = [];
        result = await repository.Remove(publisher1);
        results.Add(result.Value);
        result = await repository.Remove(publisher2);
        results.Add(result.Value);
        bool allSuccess = results.All(r => r == RepositoryOperationResult.Success);
        Assert.That(allSuccess, Is.True);
    }

    [Test, Order(3)]
    public async Task CreatePublishers_With_Data_Duplication()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        IPublishersRepository repository = provider.GetRequiredService<IPublishersRepository>();
        PublisherData dataFirst = PublisherData.Create("First publisher data");
        Publisher publisher1 = new Publisher(dataFirst);
        Publisher publisher2 = new Publisher(dataFirst);
        Result<RepositoryOperationResult> result1 = await repository.Add(publisher1);
        Result<RepositoryOperationResult> result2 = await repository.Add(publisher2);
        bool operation = result1.IsSuccess && result2.IsFailure;
        await repository.Remove(publisher1);
        Assert.That(operation, Is.True);
    }

    [Test, Order(4)]
    public async Task Create_Set_Ignore_Update_Publisher_Test()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        IPublishersRepository repository = provider.GetRequiredService<IPublishersRepository>();
        PublisherData data = PublisherData.Create("Some data");
        Publisher publisher = new Publisher(data);
        Result<RepositoryOperationResult> result = await repository.Add(publisher);
        IEnumerable<Publisher> publishers = await repository.GetAll();
        Publisher createdFirst = publishers.First();
        createdFirst.MakePublisherIgnored();
        result = await repository.Update(createdFirst);
        publishers = await repository.GetAll();
        Publisher updatedFirst = publishers.First();
        bool operation = result.IsSuccess && updatedFirst.IsIgnored;
        await repository.Remove(publisher);
        Assert.That(operation, Is.True);
    }
}
