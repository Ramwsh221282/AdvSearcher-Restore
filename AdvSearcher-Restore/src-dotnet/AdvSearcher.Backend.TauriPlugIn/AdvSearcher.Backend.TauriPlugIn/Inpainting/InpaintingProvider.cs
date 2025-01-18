using System.Diagnostics;
using AdvSearcher.Backend.TauriPlugIn.Controllers;
using AdvSearcher.Image.Inpainting.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Backend.TauriPlugIn.Inpainting;

public sealed class InpaintingProvider
{
    private readonly IServiceScopeFactory _factory;

    public InpaintingProvider(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public void ProcessImage(
        string workspaceDirectoryName,
        InpaintingDataRequest request,
        Action<int> maxProgress,
        Action<int> currentProgress
    )
    {
        using IServiceScope scope = _factory.CreateScope();
        IServiceProvider provider = scope.ServiceProvider;
        IInpaintingProcessor processor = provider.GetRequiredService<IInpaintingProcessor>();
        int current = 0;
        maxProgress.Invoke(request.Data.Length);
        currentProgress.Invoke(current);
        for (int index = 0; index < request.Data.Length; index++)
        {
            byte[] image = request.Data[index].StockImageBytes;
            byte[] mask = request.Data[index].MaskImageBytes;
            string imageFileName = $@"photo_{index}.png";
            string maskFileName = $@"mask_{index}.png";
            InpaintingWorkspace workspace = new InpaintingWorkspace(
                workspaceDirectoryName,
                image,
                mask,
                imageFileName,
                maskFileName
            );
            processor.ProcessImage(workspace);
            current = current + 1;
            currentProgress.Invoke(current);
        }
        processor.Dispose();
        maxProgress.Invoke(0);
        currentProgress.Invoke(0);
        string path = $@"{Environment.CurrentDirectory}\Inpainting\{workspaceDirectoryName}";
        OpenDirectory(path);
    }

    private void OpenDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                    Verb = "open",
                }
            );
        }
    }
}
