using AdvSearcher.VK.Parser.Components.Responses;

namespace AdvSearcher.VK.Parser.Components;

public sealed class VkGroupInfo
{
    public string GroupUrl { get; init; }
    public int GroupId { get; init; }

    public VkGroupInfo(VkGroupResponse response, string url)
    {
        var strArray = response.Items[0].ToString().Split(' ');
        GroupId = int.Parse(strArray[^1]);
        GroupUrl = url;
    }
}
