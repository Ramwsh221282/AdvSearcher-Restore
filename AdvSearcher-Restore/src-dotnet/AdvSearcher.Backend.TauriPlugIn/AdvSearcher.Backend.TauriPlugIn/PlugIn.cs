using System.Globalization;
using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Backend.TauriPlugIn.Controllers;
using AdvSearcher.Backend.TauriPlugIn.Inpainting;
using AdvSearcher.Backend.TauriPlugIn.MessageListener;
using AdvSearcher.FileSystem.SDK.Contracts;
using AdvSearcher.Image.Inpainting.SDK;
using AdvSearcher.MachineLearning.SDK;
using AdvSearcher.Parser.SDK.DependencyInjection;
using AdvSearcher.Persistance.SDK;
using AdvSearcher.Publishing.SDK.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn;

public class PlugIn : IPlugIn
{
    public void Initialize(IServiceCollection services)
    {
        try
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            IMessageListener consoleListener = new ConsoleMessageListener();
            CompositeMessageListener composite = new CompositeMessageListener();
            composite.AddListener(consoleListener);
            services.AddSingleton<IMessageListener>(composite);
            services.AddParserSDK();
            services.AddPersistanceSDK();
            services.LoadFileSystemPlugins();
            services.AddPublishingServices();
            services.LoadML();
            services.AddSingleton<PersistanceServiceFactory>();
            services.AddSingleton<SettingsController>();
            services.AddSingleton<ParserLinksController>();
            services.AddSingleton<ParsingController>();
            services.AddSingleton<ParsedDataController>();
            services.AddScoped<AdvertisementsFileSystemController>();
            services.AddSingleton<PublishingLinksController>();
            services.AddSingleton<PublishingDataController>();
            services.AddSingleton<PublishDataController>();
            services.AddSingleton<ImageInpaintingController>();
            services.LoadInpainting();
            services.AddSingleton<InpaintingProvider>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}
