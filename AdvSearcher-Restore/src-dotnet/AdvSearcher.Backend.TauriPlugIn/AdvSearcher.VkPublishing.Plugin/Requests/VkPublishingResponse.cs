namespace AdvSearcher.VkPublishing.Plugin.Requests;

public abstract record VkPublishingResponse;

public sealed record GroupIdResponse(string Id) : VkPublishingResponse;

public sealed record UploadUrlResponse(string UploadUrl) : VkPublishingResponse;

public sealed record UploadServerResponse(string Server, string Photo, string Hash)
    : VkPublishingResponse;

public sealed record CommitedPhotoResponse(string Id, string OwnerId) : VkPublishingResponse;

public sealed record VkScreenNameResponse(string ScreenName);
