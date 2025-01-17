using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;

namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

internal sealed class InputVectorizer
{
    private const int MaxHeight = 512;
    private const int MaxWidth = 512;
    private readonly Size _size;

    public InputVectorizer()
    {
        _size = new Size(MaxWidth, MaxHeight);
    }

    public Tensor<float> CreateImageTensor(Mat imageMat)
    {
        using (Mat dest = new Mat())
        {
            Cv2.Resize(imageMat, dest, _size, interpolation: InterpolationFlags.Cubic);
            imageMat.Dispose();
            Tensor<float> imageTensor = new DenseTensor<float>(
                new[] { 1, 3, dest.Height, dest.Width }
            );
            for (int i0 = 0; i0 < dest.Height; ++i0)
            {
                for (int i1 = 0; i1 < dest.Width; ++i1)
                {
                    Vec3b pixel = dest.At<Vec3b>(i0, i1);
                    imageTensor[0, 0, i0, i1] = pixel[0] / 255.0f;
                    imageTensor[0, 1, i0, i1] = pixel[1] / 255.0f;
                    imageTensor[0, 2, i0, i1] = pixel[2] / 255.0f;
                }
            }
            return imageTensor;
        }
    }

    public Tensor<float> CreateMaskTensor(Mat maskMat)
    {
        Mat resized = new Mat();
        Cv2.Resize(maskMat, resized, _size, 0, 0, InterpolationFlags.Cubic);
        maskMat.Dispose();
        Tensor<float> tensor = new DenseTensor<float>(new[] { 1, 1, MaxWidth, MaxHeight });
        Size size = new Size(11, 11);
        using (Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, size))
            Cv2.Dilate(resized, resized, element);
        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                float vector = resized.At<Vec3b>(y, x)[0];
                if (vector > 127)
                    tensor[0, 0, y, x] = 1.0f;
                else
                    tensor[0, 0, y, x] = 0.0f;
            }
        }
        resized.Dispose();
        return tensor;
    }
}
