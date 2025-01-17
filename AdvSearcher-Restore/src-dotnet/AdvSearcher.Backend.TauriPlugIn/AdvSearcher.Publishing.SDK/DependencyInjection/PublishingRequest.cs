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
        ServiceUrl url
    )
    {
        Task operation = request switch
        {
            SocialMediaPublishRequest socialMedia => provider
                .GetService(socialMedia.ServiceName)
                .Publish(socialMedia.Files, url),
            EmailPublishRequest emailRequest => provider
                .GetMailingClient(emailRequest.ServiceName)
                .Send(emailRequest.Files, emailRequest.Address),
            WhatsAppPublishRequest waRequest => provider
                .GetWhatsAppService(waRequest.ServiceName)
                .Publish(waRequest.Files, waRequest.PhoneNumber),
            _ => Task.CompletedTask,
        };
        await operation;
    }
}
