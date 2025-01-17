using AdvSearcher.Image.Inpainting.SDK;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;

namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

internal sealed class InpaintingEngine : IDisposable
{
    private static readonly string Path =
        $@"{Environment.CurrentDirectory}/Plugins/Inpainting/lama_fp32.onnx";
    private readonly InferenceSession _session;

    public InpaintingEngine() => _session = new InferenceSession(Path);

    public void InpaintImage(InpaintingWorkspace workspace)
    {
        Input image = new Input(workspace.StockFilePath, workspace.MaskFilePath);
        Mat processed = GetProcessedImage(image);
        string result = System.IO.Path.Combine(workspace.SampleRoot, "inpainted.png");
        SaveImage(processed, result);
        File.Delete(workspace.StockFilePath);
        File.Move(result, workspace.StockFilePath);
        File.Delete(workspace.MaskFilePath);
        processed.Dispose();
    }

    private Mat GetProcessedImage(Input image)
    {
        using (
            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> outputTensor = _session.Run(
                image.InputTensors
            )
        )
        {
            return new OutputImageCreator(new Output(outputTensor)).PostProcessImage();
        }
    }

    private void SaveImage(Mat processed, string path) => Cv2.ImWrite(path, processed);

    public void Dispose() => _session.Dispose();
}
