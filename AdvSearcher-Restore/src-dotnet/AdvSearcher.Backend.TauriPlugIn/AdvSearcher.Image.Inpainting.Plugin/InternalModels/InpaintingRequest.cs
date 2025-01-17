namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

public sealed class InpaintingRequest
{
    public required byte[] MaskImageBytes;
    public required byte[] StockImageBytes;
}
