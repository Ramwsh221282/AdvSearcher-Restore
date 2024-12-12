namespace Advsearcher.Infrastructure.VKParser;

public sealed record VkOptions(
    string ServiceAccessToken,
    string OAuthAccessToken,
    string ApiVesrion = "5.131"
);
