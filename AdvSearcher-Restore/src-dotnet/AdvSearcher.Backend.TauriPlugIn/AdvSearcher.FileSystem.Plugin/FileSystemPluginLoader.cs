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
        _listener.Publish("Подгрузка плагинов файловой системы...");
        services.AddSingleton<IFileSystem, AdvertisementFileSystem>();
        IServiceProvider provider = services.BuildServiceProvider();
        IFileSystem[] plugins = provider.GetServices<IFileSystem>().ToArray();
        if (plugins.Any())
        {
            _listener.Publish("Плагины файловой системы добавлен");
            _listener.Publish($"Количество плагинов файловой системы: {plugins.Length}");
        }
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
