using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;

namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

internal sealed class Input
{
    public List<NamedOnnxValue> InputTensors { get; init; }

    public int StockHeight { get; init; }
    public int StockWidth { get; init; }

    public Input(string stockImagePath, string maskPath)
    {
        using (Mat imageMat = new Mat(stockImagePath))
        {
            using (Mat maskMat = new Mat(maskPath))
            {
                StockHeight = imageMat.Height;
                StockWidth = imageMat.Width;
                InputVectorizer vectorizer = new InputVectorizer();
                Tensor<float> imageVector = vectorizer.CreateImageTensor(imageMat);
                Tensor<float> maskVector = vectorizer.CreateMaskTensor(maskMat);
                InputTensors = new List<NamedOnnxValue>()
                {
                    NamedOnnxValue.CreateFromTensor("image", imageVector),
                    NamedOnnxValue.CreateFromTensor("mask", maskVector),
                };
            }
        }
    }
}
