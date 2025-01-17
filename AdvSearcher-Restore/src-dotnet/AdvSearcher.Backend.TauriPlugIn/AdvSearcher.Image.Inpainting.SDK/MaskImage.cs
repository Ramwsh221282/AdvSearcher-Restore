using System.Drawing.Imaging;

namespace AdvSearcher.Image.Inpainting.SDK;

public sealed class MaskImage
{
    public string MaskFile { get; }

    public MaskImage(byte[] maskImage, string maskFileName, InpaintingWorkspace workspace)
    {
        MaskFile = CreateTemporaryImageMask(maskImage, maskFileName, workspace);
        CreateAdjustedDpiImageMask(maskFileName, workspace);
    }

    private string CreateTemporaryImageMask(
        byte[] maskImage,
        string maskFileName,
        InpaintingWorkspace workspace
    )
    {
        string temporaryMaskPath = Path.Combine(workspace.SampleRoot, maskFileName);
        using (MemoryStream stream = new MemoryStream(maskImage))
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(stream))
            img.Save(temporaryMaskPath, ImageFormat.Png);
        return temporaryMaskPath;
    }

    private void CreateAdjustedDpiImageMask(string maskFileName, InpaintingWorkspace workspace)
    {
        string adjustedDpiMaskPath = Path.Combine(workspace.SampleRoot, "mask.png");
        string currentMaskPath = Path.Combine(workspace.SampleRoot, maskFileName);
        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(currentMaskPath))
        {
            bitmap.SetResolution(300f, 300f);
            bitmap.Save(adjustedDpiMaskPath);
        }
        File.Delete(MaskFile);
        File.Move(adjustedDpiMaskPath, MaskFile);
    }
}
