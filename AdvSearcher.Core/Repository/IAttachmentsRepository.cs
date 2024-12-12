using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository;

public interface IAttachmentsRepository
{
    Task<Result<AdvertisementAttachment>> AddAttachment(AdvertisementAttachment attachment);
    Task<Result<IEnumerable<AdvertisementAttachment>>> AddAttachments(
        IEnumerable<AdvertisementAttachment> attachments
    );
}
