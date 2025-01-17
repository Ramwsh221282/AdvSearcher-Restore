namespace AdvSearcher.Image.Inpainting.SDK;

public sealed class InpaintingRequest
{
    public required byte[] MaskImageBytes;
    public required byte[] StockImageBytes;
}
