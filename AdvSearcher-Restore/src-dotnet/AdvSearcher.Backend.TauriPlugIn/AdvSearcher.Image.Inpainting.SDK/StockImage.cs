using System.Drawing.Imaging;

namespace AdvSearcher.Image.Inpainting.SDK;

public sealed class StockImage
{
    public string StockFile { get; }

    public StockImage(
        byte[] stockImageBytes,
        string stockImageFileName,
        InpaintingWorkspace workspace
    )
    {
        StockFile = CreateStockImage(stockImageBytes, stockImageFileName, workspace);
        CreateAdjustedStockImage(stockImageFileName, workspace);
    }

    private string CreateStockImage(
        byte[] stockImageBytes,
        string stockImageFileName,
        InpaintingWorkspace workspace
    )
    {
        string stockImage = Path.Combine(workspace.SampleRoot, stockImageFileName);
        using (MemoryStream stream = new MemoryStream(stockImageBytes))
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(stream))
            img.Save(stockImage, ImageFormat.Png);
        return stockImage;
    }

    private void CreateAdjustedStockImage(string stockImageFilePath, InpaintingWorkspace workspace)
    {
        string adjustedStockImagePath = Path.Combine(workspace.SampleRoot, "image.png");
        string currentImagePath = Path.Combine(workspace.SampleRoot, stockImageFilePath);
        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(currentImagePath))
        {
            bitmap.SetResolution(300f, 300f);
            bitmap.Save(adjustedStockImagePath);
        }
        File.Delete(StockFile);
        File.Move(adjustedStockImagePath, StockFile);
    }
}
