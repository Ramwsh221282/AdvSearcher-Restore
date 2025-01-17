using AdvSearcher.Backend.TauriPlugIn.Inpainting;
using AdvSearcher.Image.Inpainting.SDK;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public sealed record InpaintingDataRequest(InpaintingRequest[] Data);

public class ImageInpaintingController
{
    private readonly InpaintingProvider _provider;

    public ImageInpaintingController(InpaintingProvider provider) => _provider = provider;

    public void InpaintImages(InpaintingDataRequest request)
    {
        try
        {
            string workspaceDirectoryName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            _provider.ProcessImage(workspaceDirectoryName, request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
