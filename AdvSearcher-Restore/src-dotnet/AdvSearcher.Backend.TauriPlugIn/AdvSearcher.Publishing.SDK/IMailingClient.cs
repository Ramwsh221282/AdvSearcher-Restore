using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK;

public sealed record MailingAddress(string Email, string Subject);

public interface IMailingClient
{
    Task Send(IEnumerable<AdvertisementFileResponse> files, MailingAddress address);
}
