namespace AdvSearcher.Image.Inpainting.SDK;

public sealed class InpaintingWorkspace
{
    private const string WorkSpaceRoot = "Inpainting";
    public string SampleRoot { get; }
    private readonly MaskImage _mask;
    private readonly StockImage _stock;

    public string MaskFilePath => _mask.MaskFile;
    public string StockFilePath => _stock.StockFile;

    public InpaintingWorkspace(
        string workspaceDirectory,
        byte[] stockImage,
        byte[] maskImage,
        string stockImageFileName,
        string maskImageFileName
    )
    {
        CreateWorkSpaceRootIfNotExists();
        SampleRoot = Path.Combine(WorkSpaceRoot, workspaceDirectory);
        CreateSampleRoot();
        _mask = new MaskImage(maskImage, maskImageFileName, this);
        _stock = new StockImage(stockImage, stockImageFileName, this);
    }

    private void CreateWorkSpaceRootIfNotExists()
    {
        if (!Directory.Exists(WorkSpaceRoot))
            Directory.CreateDirectory(WorkSpaceRoot);
    }

    private void CreateSampleRoot()
    {
        if (!Directory.Exists(SampleRoot))
            Directory.CreateDirectory(SampleRoot);
    }
}
