using AdvSearcher.Application.Contracts.Progress;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK;

public sealed record MailingAddress(string Email, string Subject);

public interface IMailingClient : IProgressable
{
    Task Send(IEnumerable<AdvertisementFileResponse> files, MailingAddress address);
}
