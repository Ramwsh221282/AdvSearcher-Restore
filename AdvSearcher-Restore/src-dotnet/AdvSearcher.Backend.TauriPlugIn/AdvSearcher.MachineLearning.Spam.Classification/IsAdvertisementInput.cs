using Microsoft.ML.Data;

namespace AdvSearcher.MachineLearning.Spam.Classification;

public class IsAdvertisementInput
{
    [LoadColumn((0))]
    public string Text { get; set; } = null!;

    [LoadColumn((0))]
    public bool Label { get; set; }
}
