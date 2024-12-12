using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

internal sealed class VkPublisher : IParsedPublisher
{
    public string Id { get; init; }
    public string Info { get; set; } = string.Empty;

    private VkPublisher(string id) => Id = id;

    public async Task<Result<bool>> InitializeName(
        IVkParserRequestFactory factory,
        RestClient client,
        VkOptions options
    )
    {
        var request = factory.CreateVkPostOwnerRequest(client, options, this);
        var name = await request.ExecuteAsync();
        if (string.IsNullOrWhiteSpace(name))
            return new Error("Не удалось получить данные об авторе поста ВК");
        Info = name;
        return true;
    }

    public static Result<VkPublisher> Create(JToken token)
    {
        var fromIdToken = token["from_id"];
        if (fromIdToken == null)
            return new Error("Не удалось получить ИД автора поста");
        var id = fromIdToken.ToString();
        return new VkPublisher(id);
    }
}
