using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Avito.Parser.Steps.FourthStep.Converters;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Models;

public sealed class CreateResponsesStep : IAvitoFastParserStep
{
    private readonly AvitoDateConverter _converter;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public CreateResponsesStep(AvitoFastParserPipeline pipeLine, IAvitoFastParserStep? next = null)
    {
        Pipeline = pipeLine;
        Next = next;
        _converter = new AvitoDateConverter();
    }

    public async Task ProcessAsync()
    {
        if (Pipeline.Total == 0)
            return;
        foreach (AvitoAdvertisement advertisement in Pipeline)
        {
            Result<IParsedAdvertisement> ad = AvitoResponseAdvertisement.Create(
                advertisement,
                _converter
            );
            Result<IParsedPublisher> publisher = AvitoResponsePublisher.Create(advertisement);
            if (ad.IsFailure || publisher.IsFailure)
                continue;
            IParsedAttachment[] attachments = AvitoResponseAttachment.Create(advertisement);
            IParserResponse response = new AvitoResponse(ad.Value, attachments, publisher.Value);
            Pipeline.Results.Add(response);
        }
        Pipeline.FilterByPublishers();
        if (Next != null)
            await Next.ProcessAsync();
    }
}
