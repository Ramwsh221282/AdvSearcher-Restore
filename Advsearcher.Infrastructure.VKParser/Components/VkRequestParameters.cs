using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Components;

internal class VkRequestParameters
{
    public string BaseUrl { get; init; }
    public string ScreenName { get; init; }

    private VkRequestParameters(ServiceUrl url)
    {
        BaseUrl = url.Value.Value;
        ScreenName = new Uri(BaseUrl).AbsolutePath.Trim('/');
    }

    public static Result<VkRequestParameters> Create(ServiceUrl url)
    {
        if (url.Mode == ServiceUrlMode.Publicatable)
            return ParserErrors.UrlIsNotForLoading;
        return new VkRequestParameters(url);
    }
}
