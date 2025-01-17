using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

internal sealed class Output
{
    public float[] PixelArray { get; }
    public Tensor<float> OutputVector { get; }

    public Output(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> vectors)
    {
        DisposableNamedOnnxValue[] values = vectors.ToArray();
        OutputVector = values[0].AsTensor<float>();
        PixelArray = OutputVector.ToArray();
    }
}
