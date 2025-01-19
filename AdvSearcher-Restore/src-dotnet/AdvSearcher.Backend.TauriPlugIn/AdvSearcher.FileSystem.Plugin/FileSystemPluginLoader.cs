using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.FileSystem.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.FileSystem.Plugin;

public sealed class FileSystemPluginLoader : IFileSystemPluginLoader, IListenable
{
    private IMessageListener _listener;

    public FileSystemPluginLoader(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection Load(IServiceCollection services)
    {
        services.AddSingleton<IFileSystem, AdvertisementFileSystem>();
        IServiceProvider provider = services.BuildServiceProvider();
        IFileSystem[] plugins = provider.GetServices<IFileSystem>().ToArray();
        if (!plugins.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR. File system plugins were not loaded");
            throw new ApplicationException();
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("File system plugins were loaded");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
