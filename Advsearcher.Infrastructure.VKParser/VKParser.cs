using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using Advsearcher.Infrastructure.VKParser.Models.Factories;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser;

internal sealed class VkParser(
    IHttpService httpService,
    IHttpClient httpClient,
    IVkOptionsProvider optionsProvider,
    IVkParserRequestFactory requestFactory,
    VkAdvertisementsFactory advertisementsFactory
) : IParser<VkParserService>
{
    // Vk client options. Service Access token and OAuth access token
    private readonly VkOptions _options = optionsProvider.Provide();

    // Results of Vk parser work
    private readonly List<IParserResponse> _results = [];

    // Results of Vk parser work
    public IReadOnlyCollection<IParserResponse> Results => _results;

    // public API to make parser work.
    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        Result<VkRequestParameters> parameters = VkRequestParameters.Create(url);
        Result<VkGroupInfo> info = await CreateVkGroupInfo(parameters);
        Result<VkItemsJson> items = await CreateVkItemsJson(info);
        await AddSuccessfullyConstructedAdvertisements(info, items);
        httpClient.Dispose();
        return Result<bool>.Success(true);
    }

    // step 1. get vk group info.
    private async Task<Result<VkGroupInfo>> CreateVkGroupInfo(
        Result<VkRequestParameters> parameters
    )
    {
        if (parameters.IsFailure)
            return parameters.Error;
        IHttpRequest request = requestFactory.CreateVkGroupOwnerIdRequest(_options, parameters);
        VKGroupInfoResponseFactory factory = new VKGroupInfoResponseFactory(
            request,
            httpClient,
            httpService
        );
        return await factory.CreateGroupInfo(parameters);
    }

    // step 2. get vk wall posts
    private async Task<Result<VkItemsJson>> CreateVkItemsJson(Result<VkGroupInfo> info)
    {
        if (info.IsFailure)
            return info.Error;
        IHttpRequest request = requestFactory.CreateWallPostRequest(_options, info.Value);
        VkItemsJsonFactory factory = new VkItemsJsonFactory(httpService, request, httpClient);
        return await factory.CreateVkItemsJson();
    }

    // step 3. construct advertisements from vk wall posts
    private async Task AddSuccessfullyConstructedAdvertisements(
        Result<VkGroupInfo> info,
        Result<VkItemsJson> items
    )
    {
        Result<List<IParserResponse>> constructed = await advertisementsFactory.Construct(
            _options,
            items,
            info
        );
        if (constructed.IsSuccess)
            _results.AddRange(constructed.Value);
    }
}
