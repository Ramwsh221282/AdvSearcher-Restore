namespace AdvSearcher.Image.Inpainting.SDK;

public interface IInpaintingProcessor : IDisposable
{
    void ProcessImage(InpaintingWorkspace workspace);
}
