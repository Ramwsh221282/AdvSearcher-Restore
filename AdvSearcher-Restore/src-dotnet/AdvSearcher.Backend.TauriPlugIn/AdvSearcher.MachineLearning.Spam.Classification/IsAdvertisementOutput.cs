using Microsoft.ML.Data;

namespace AdvSearcher.MachineLearning.Spam.Classification;

public class IsAdvertisementOutput
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}
