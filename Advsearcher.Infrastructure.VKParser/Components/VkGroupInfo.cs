using AdvSearcher.Core.Entities.ServiceUrls;
using Advsearcher.Infrastructure.VKParser.Components.Responses;

namespace Advsearcher.Infrastructure.VKParser.Components;

internal sealed class VkGroupInfo
{
    public string GroupUrl { get; init; }
    public int GroupId { get; init; }

    public VkGroupInfo(VkGroupResponse response, ServiceUrl url)
    {
        var strArray = response.Items[0].ToString().Split(' ');
        GroupId = int.Parse(strArray[^1]);
        GroupUrl = url.Url.Value;
    }
}
