using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Repository.InMemoryRepository;

public sealed class InMemoryAttachmentsRepository : IAttachmentsRepository
{
    private readonly List<AdvertisementAttachment> _attachments = [];

    public async Task<Result<AdvertisementAttachment>> AddAttachment(
        AdvertisementAttachment attachment
    )
    {
        _attachments.Add(attachment);
        return await Task.FromResult(attachment);
    }

    public async Task<Result<IEnumerable<AdvertisementAttachment>>> AddAttachments(
        IEnumerable<AdvertisementAttachment> attachments
    )
    {
        _attachments.AddRange(attachments);
        return await Task.FromResult(
            Result<IEnumerable<AdvertisementAttachment>>.Success(attachments)
        );
    }
}
