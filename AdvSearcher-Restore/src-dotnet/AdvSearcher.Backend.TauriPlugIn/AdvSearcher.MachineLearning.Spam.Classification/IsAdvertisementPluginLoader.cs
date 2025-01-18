using AdvSearcher.MachineLearning.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MachineLearning.Spam.Classification;

public sealed class IsAdvertisementPluginLoader : IMLPluginLoader
{
    public IServiceCollection LoadML(IServiceCollection services)
    {
        services = services.AddSingleton<ISpamClassifier, IsAdvertisementClassifier>();
        return services;
    }
}
