namespace Advsearcher.Infrastructure.VKParser;

internal sealed class TestVkOptionsProvider : IVkOptionsProvider
{
    private const string OAuthToken =
        "vk1.a.d2VrBiqiNCRqH51PROF2HR8G3xpfok5NIEg8bGa7x7aPuIOK5-Enr8Es3mtTOZdVxixeFmqukBIuFrf82qLquZtzaHXT17kimqfNqI4O12tRQak55R5MyJfp3ZKNHSRWrWPreUbFmm397R_IXpToVGQ_T0pc7ef14AHj9lgURa7ETz1IaOqdy4Rawd_1ve6N0vcS69EwJdK8BGQbaRv6KQ";
    private const string ServiceToken =
        "fd04e34ffd04e34ffd04e34f27fe12b5f8ffd04fd04e34f985cba7537322b2a92927c95";

    private readonly VkOptions _options = new(ServiceToken, OAuthToken);

    public VkOptions Provide() => _options;
}
