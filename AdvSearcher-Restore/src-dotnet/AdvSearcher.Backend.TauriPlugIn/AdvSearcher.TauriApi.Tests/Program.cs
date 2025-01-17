using AdvSearcher.Backend.TauriPlugIn;
using AdvSearcher.Backend.TauriPlugIn.Controllers;
using AdvSearcher.Image.Inpainting.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.TauriApi.Tests
{
    class Program
    {
        static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            PlugIn plugIn = new PlugIn();
            plugIn.Initialize(serviceCollection);
            IServiceProvider provider = serviceCollection.BuildServiceProvider();
            ImageInpaintingController controller =
                provider.GetRequiredService<ImageInpaintingController>();

            InpaintingDataRequest request = new InpaintingDataRequest(
                [
                    new InpaintingRequest()
                    {
                        MaskImageBytes = File.ReadAllBytes("mask.png"),
                        StockImageBytes = File.ReadAllBytes("image.png"),
                    },
                    new InpaintingRequest()
                    {
                        MaskImageBytes = File.ReadAllBytes("mask.png"),
                        StockImageBytes = File.ReadAllBytes("image.png"),
                    },
                    new InpaintingRequest()
                    {
                        MaskImageBytes = File.ReadAllBytes("mask.png"),
                        StockImageBytes = File.ReadAllBytes("image.png"),
                    },
                ]
            );

            controller.InpaintImages(request);

            Console.WriteLine("Finished");
        }
    }
}
