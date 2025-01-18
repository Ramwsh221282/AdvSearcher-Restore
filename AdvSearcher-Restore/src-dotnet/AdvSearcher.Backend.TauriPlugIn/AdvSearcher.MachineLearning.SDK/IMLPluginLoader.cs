using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MachineLearning.SDK;

public interface IMLPluginLoader
{
    IServiceCollection LoadML(IServiceCollection services);
}
