using AdvSearcher.Core.Entities.ServiceUrls;

namespace Advsearcher.Infrastructure.VKParser.Components;

public class VkRequestParameters
{
    public string BaseUrl { get; init; }
    public string ScreenName { get; init; }

    public VkRequestParameters(ServiceUrl url)
    {
        BaseUrl = url.Url.Value;
        ScreenName = new Uri(BaseUrl).AbsolutePath.Trim('/');
    }
}
