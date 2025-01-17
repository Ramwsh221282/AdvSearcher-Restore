using AdvSearcher.Image.Inpainting.Plugin.InternalModels;
using AdvSearcher.Image.Inpainting.SDK;

namespace AdvSearcher.Image.Inpainting.Plugin.ApiModels;

public sealed class InpaintingProcessor : IInpaintingProcessor
{
    private readonly InpaintingEngine _engine;

    public InpaintingProcessor() => _engine = new InpaintingEngine();

    public void Dispose() => _engine.Dispose();

    public void ProcessImage(InpaintingWorkspace workspace) => _engine.InpaintImage(workspace);
}
