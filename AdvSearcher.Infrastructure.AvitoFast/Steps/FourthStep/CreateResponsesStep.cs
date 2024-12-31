using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters;
using AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Models;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep;

internal sealed class CreateResponsesStep : IAvitoFastParserStep
{
    private readonly ParserConsoleLogger _logger;
    private readonly AvitoDateConverter _converter;
    public AvitoFastParserPipeline Pipeline { get; }
    public IAvitoFastParserStep? Next { get; }

    public CreateResponsesStep(
        AvitoFastParserPipeline pipeLine,
        ParserConsoleLogger logger,
        IAvitoFastParserStep? next = null
    )
    {
        Pipeline = pipeLine;
        _logger = logger;
        Next = next;
        _converter = new AvitoDateConverter(logger);
    }

    public async Task ProcessAsync()
    {
        _logger.Log("Start converting results into responses");
        if (Pipeline.Total == 0)
        {
            _logger.Log("No results in pipeline to convert into resposes. Stopping process");
            return;
        }

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
        _logger.Log("Response creation finished");
        if (Next != null)
        {
            _logger.Log("Processing next");
            await Next.ProcessAsync();
        }
    }
}
