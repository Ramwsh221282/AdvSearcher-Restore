using AdvSearcher.Backend.TauriPlugIn.Inpainting;
using AdvSearcher.Image.Inpainting.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public sealed record InpaintingDataRequest(InpaintingRequest[] Data);

public class ImageInpaintingController
{
    private const string MaxProgressListener = "inpainting-max-progress";
    private const string CurrentProgressListener = "inpainting-current-progress";
    private readonly IEventPublisher _publisher;
    private readonly InpaintingProvider _provider;

    public ImageInpaintingController(InpaintingProvider provider, IEventPublisher publisher)
    {
        _provider = provider;
        _publisher = publisher;
    }

    public void InpaintImages(InpaintingDataRequest request)
    {
        try
        {
            Action<int> currentProgress = (value) =>
                _publisher.Publish(CurrentProgressListener, value);
            Action<int> maxProgress = (value) => _publisher.Publish(MaxProgressListener, value);
            string workspaceDirectoryName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            _provider.ProcessImage(workspaceDirectoryName, request, currentProgress, maxProgress);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}
