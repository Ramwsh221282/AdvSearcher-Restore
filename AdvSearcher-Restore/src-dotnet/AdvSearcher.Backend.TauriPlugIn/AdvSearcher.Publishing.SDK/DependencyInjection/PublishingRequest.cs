using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK.DependencyInjection;

public abstract record PublishingRequest;

public sealed record SocialMediaPublishRequest(
    IEnumerable<AdvertisementFileResponse> Files,
    string ServiceName
) : PublishingRequest;

public sealed record EmailPublishRequest(
    IEnumerable<AdvertisementFileResponse> Files,
    string ServiceName,
    MailingAddress Address
) : PublishingRequest;

public sealed record WhatsAppPublishRequest(
    IEnumerable<AdvertisementFileResponse> Files,
    string ServiceName,
    WhatsAppSendRequest PhoneNumber
) : PublishingRequest;

public static class PublishingRequestExtensions
{
    public static async Task Handle(
        this PublishingRequest request,
        PublishingServiceProvider provider,
        ServiceUrl url,
        Action<int> currentProgress,
        Action<int> maxProgress
    )
    {
        Task operation = request switch
        {
            SocialMediaPublishRequest socialMedia => Task.Run(() =>
            {
                IPublishingService service = provider.GetService(socialMedia.ServiceName);
                service.SetCurrentProgressValuePublisher(currentProgress);
                service.SetMaxProgressValuePublisher(maxProgress);
                service.Publish(socialMedia.Files, url);
            }),
            EmailPublishRequest emailRequest => Task.Run(() =>
            {
                IMailingClient client = provider.GetMailingClient(emailRequest.ServiceName);
                client.SetCurrentProgressValuePublisher(currentProgress);
                client.SetMaxProgressValuePublisher(maxProgress);
                client.Send(emailRequest.Files, emailRequest.Address);
            }),
            WhatsAppPublishRequest waRequest => Task.Run(() =>
            {
                IWhatsAppSender sender = provider.GetWhatsAppService(waRequest.ServiceName);
                sender.SetCurrentProgressValuePublisher(currentProgress);
                sender.SetMaxProgressValuePublisher(maxProgress);
                sender.Publish(waRequest.Files, waRequest.PhoneNumber);
            }),
            _ => Task.CompletedTask,
        };
        await operation;
    }
}
