using AdvSearcher.Image.Inpainting.Plugin.ApiModels;
using AdvSearcher.Image.Inpainting.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Image.Inpainting.Plugin.DependencyInjection;

public sealed class OnnxInpainterPlugin
{
    public IServiceCollection Load(IServiceCollection servives)
    {
        servives = servives.AddScoped<IInpaintingProcessor, InpaintingProcessor>();
        return servives;
    }
}
