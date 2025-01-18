using AdvSearcher.Application.Contracts.Progress;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK;

public interface IPublishingService : IProgressable
{
    Task Publish(IEnumerable<AdvertisementFileResponse> selectedFiles, ServiceUrl serviceUrl);
}
