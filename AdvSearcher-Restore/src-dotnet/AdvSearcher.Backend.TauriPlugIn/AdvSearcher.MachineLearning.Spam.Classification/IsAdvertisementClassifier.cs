using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.MachineLearning.SDK;
using Microsoft.ML;

namespace AdvSearcher.MachineLearning.Spam.Classification;

public sealed class IsAdvertisementClassifier : ISpamClassifier
{
    private static string ModelPath =
        $@"{Environment.CurrentDirectory}\Plugins\ML\SpamClassification.zip";
    private PredictionEngine<IsAdvertisementInput, IsAdvertisementOutput> _engine;
    private bool isDisposed;

    public IsAdvertisementClassifier()
    {
        MLContext _context = new MLContext();
        _engine = _context.Model.CreatePredictionEngine<
            IsAdvertisementInput,
            IsAdvertisementOutput
        >(_context.Model.Load(ModelPath, out _));
    }

    private void InstantiateIfDisposed()
    {
        if (isDisposed)
        {
            MLContext _context = new MLContext();
            _engine = _context.Model.CreatePredictionEngine<
                IsAdvertisementInput,
                IsAdvertisementOutput
            >(_context.Model.Load(ModelPath, out _));
            isDisposed = false;
        }
    }

    public void Dispose()
    {
        if (isDisposed)
            return;
        _engine.Dispose();
        isDisposed = true;
    }

    public bool IsSpam(Advertisement advertisement)
    {
        InstantiateIfDisposed();
        string text = advertisement.Content.Content;
        text = text.FormatToLower().CleanString();
        IsAdvertisementInput input = new IsAdvertisementInput() { Text = text };
        IsAdvertisementOutput output = _engine.Predict(input);
        float score = output.Score;
        if (score < 0.0)
        {
            Console.WriteLine($"Score: {score}");
            Console.WriteLine("Is spam");
            return true;
        }
        Console.WriteLine("Not a spam");
        return false;
    }
}
