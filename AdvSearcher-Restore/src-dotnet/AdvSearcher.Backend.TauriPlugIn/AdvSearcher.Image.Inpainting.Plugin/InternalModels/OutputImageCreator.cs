using OpenCvSharp;

namespace AdvSearcher.Image.Inpainting.Plugin.InternalModels;

internal sealed class OutputImageCreator
{
    private readonly Output _output;
    private readonly int _batchSize;
    private readonly int _channels;
    private readonly int _height;
    private readonly int _width;

    public OutputImageCreator(Output output)
    {
        _output = output;
        _batchSize = _output.OutputVector.Dimensions[0];
        _channels = _output.OutputVector.Dimensions[1];
        _height = _output.OutputVector.Dimensions[2];
        _width = _output.OutputVector.Dimensions[3];
    }

    public Mat PostProcessImage()
    {
        float[,,] imageData = new float[_height, _width, _channels];
        SetPixels(imageData);
        SetColors(imageData);
        return new Mat(_height, _width, MatType.CV_32FC3, imageData);
    }

    private void SetPixels(float[,,] imageData)
    {
        for (int b = 0; b < _batchSize; b++)
        {
            for (int c = 0; c < _channels; c++)
            {
                for (int h = 0; h < _height; h++)
                {
                    for (int w = 0; w < _width; w++)
                    {
                        int index =
                            b * _channels * _height * _width
                            + c * _height * _width
                            + h * _width
                            + w;
                        imageData[h, w, c] = _output.PixelArray[index];
                    }
                }
            }
        }
    }

    private void SetColors(float[,,] imageData)
    {
        for (int h = 0; h < _height; h++)
        {
            for (int w = 0; w < _width; w++)
            {
                for (int c = 0; c < _channels; c++)
                {
                    imageData[h, w, c] = Math.Max(0, Math.Min(256, imageData[h, w, c]));
                }
            }
        }
    }
}
