using AdvSearcher.MachineLearning.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MachineLearning.Spam.Classification;

public sealed class IsAdvertisementPluginLoader : IMLPluginLoader
{
    public IServiceCollection LoadML(IServiceCollection services)
    {
        services = services.AddSingleton<ISpamClassifier, IsAdvertisementClassifier>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Anti spam classification ML Model loaded");
        return services;
    }
}
