using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK;

public interface IPublishingService
{
    Task Publish(IEnumerable<AdvertisementFileResponse> selectedFiles, ServiceUrl serviceUrl);
}
