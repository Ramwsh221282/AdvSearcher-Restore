using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using Advsearcher.Infrastructure.VKParser.Models.Factories;
using Advsearcher.Infrastructure.VKParser.Models.HttpSenders;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser;

public sealed class VkParser(
    IVkHttpSender sender,
    IVkOptionsProvider optionsProvider,
    IVkParserRequestFactory factory,
    IAdvertisementDateConverter<VkParser> converter
) : IParser<VkParser>
{
    private VkAdvertisementsFactory? _factory;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        if (url.Mode == ServiceUrlMode.Publicatable)
            return ParserErrors.UrlIsNotForLoading;
        using (var client = sender.ProvideRestClient())
        {
            var options = optionsProvider.Provide();
            var method = new VkParsingMethod(factory);
            var info = await method.GetGroupInfoAsync(client, options, url);
            if (info.IsFailure)
                return info.Error;
            var items = await method.GetVkWallItemsJsonAsync(client, options, info);
            if (items.IsFailure)
                return items.Error;
            await ConstructAdvertisements(client, options, info, items);
        }
        return Result<bool>.Success(true);
    }

    public IReadOnlyCollection<IParserResponse> Results =>
        _factory == null ? Array.Empty<IParserResponse>() : _factory.Data;

    private async Task ConstructAdvertisements(
        RestClient client,
        VkOptions options,
        VkGroupInfo info,
        VkItemsJson items
    )
    {
        _factory = new VkAdvertisementsFactory(client, options, items, info);
        await _factory.Construct(factory, converter);
    }
}
