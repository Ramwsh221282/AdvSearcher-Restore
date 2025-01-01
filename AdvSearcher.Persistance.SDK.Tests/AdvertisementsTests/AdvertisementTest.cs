using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK.Tests.AdvertisementsTests;

[TestFixture]
[Category("Advertisements")]
public class AdvertisementTest
{
    private readonly IServiceCollection _services;

    public AdvertisementTest()
    {
        _services = new ServiceCollection();
        _services.AddPersistanceSDK();
    }

    [Test, Order(1)]
    public async Task Create_And_Remove_Advertisement()
    {
        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        PublisherData data = PublisherData.Create("Publisher of ad");
        Publisher publisher = new Publisher(data);

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName,
            url,
            createdDate,
            type
        );

        List<AdvertisementAttachment> attachments = [];
        attachments.Add(
            new AdvertisementAttachment(
                AdvertisementAttachmentSourceUrl.Create("Attachment1"),
                advertisement
            )
        );

        advertisement.SetAttachments(attachments);
        advertisement.SetPublisher(publisher);

        IServiceProvider serviceProvider = _services.BuildServiceProvider();
        IAdvertisementsRepository repository =
            serviceProvider.GetRequiredService<IAdvertisementsRepository>();
        Result<RepositoryOperationResult> result = await repository.Add(advertisement);
        await repository.Delete(advertisement);
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test, Order(2)]
    public async Task Create_Advertisement_With_Existing_Publisher()
    {
        IServiceProvider serviceProvider = _services.BuildServiceProvider();
        IAdvertisementsRepository repository =
            serviceProvider.GetRequiredService<IAdvertisementsRepository>();
        IPublishersRepository publishersRepository =
            serviceProvider.GetRequiredService<IPublishersRepository>();

        PublisherData data = PublisherData.Create("Publisher of ad");
        Publisher publisher = new Publisher(data);

        await publishersRepository.Add(publisher);

        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName,
            url,
            createdDate,
            type
        );

        List<AdvertisementAttachment> attachments = [];
        attachments.Add(
            new AdvertisementAttachment(
                AdvertisementAttachmentSourceUrl.Create("Attachment1"),
                advertisement
            )
        );

        advertisement.SetAttachments(attachments);
        advertisement.SetPublisher(publisher);

        Result<RepositoryOperationResult> result = await repository.Add(advertisement);
        await repository.Delete(advertisement);
        await publishersRepository.Remove(publisher);
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test, Order(3)]
    public async Task Create_Advertisement_Duplicate()
    {
        IServiceProvider serviceProvider = _services.BuildServiceProvider();
        IAdvertisementsRepository repository =
            serviceProvider.GetRequiredService<IAdvertisementsRepository>();
        IPublishersRepository publishersRepository =
            serviceProvider.GetRequiredService<IPublishersRepository>();

        PublisherData data = PublisherData.Create("Publisher of ad");
        Publisher publisher = new Publisher(data);

        await publishersRepository.Add(publisher);

        AdvertisementId id = AdvertisementId.Create(123);
        AdvertisementContent content = AdvertisementContent.Create("Some content");
        AdvertisementPublishedDate publishedDate = AdvertisementPublishedDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        AdvertisementServiceName serviceName = AdvertisementServiceName.Create("Some Service");
        AdvertisementUrl url = AdvertisementUrl.Create("Some url");
        CreationDate createdDate = CreationDate.Create(DateOnly.FromDateTime(DateTime.Now));
        AdvertisementType type = AdvertisementType.Create("Some type");

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            serviceName,
            url,
            createdDate,
            type
        );

        List<AdvertisementAttachment> attachments = [];
        attachments.Add(
            new AdvertisementAttachment(
                AdvertisementAttachmentSourceUrl.Create("Attachment1"),
                advertisement
            )
        );

        advertisement.SetAttachments(attachments);
        advertisement.SetPublisher(publisher);

        Result<RepositoryOperationResult> result = await repository.Add(advertisement);
        Result<RepositoryOperationResult> result2 = await repository.Add(advertisement);
        await repository.Delete(advertisement);
        await publishersRepository.Remove(publisher);
        Assert.That(result2.IsFailure, Is.True);
    }
}
