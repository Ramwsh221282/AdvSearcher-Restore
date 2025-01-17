namespace AdvSearcher.VK.Parser;

public sealed record VKOptions(
    string ServiceAccessToken,
    string OAuthAccessToken,
    string ApiVersion = "5.131"
);
