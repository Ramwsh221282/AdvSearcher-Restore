using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.FileSystem.SDK.Contracts;

public interface IFileSystemPluginLoader
{
    IServiceCollection Load(IServiceCollection collection);
}
