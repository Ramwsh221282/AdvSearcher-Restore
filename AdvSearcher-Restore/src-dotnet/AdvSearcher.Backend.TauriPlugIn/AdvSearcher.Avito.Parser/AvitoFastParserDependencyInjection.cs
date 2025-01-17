using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Avito.Parser.Steps;
using AdvSearcher.Avito.Parser.Steps.FirstStep;
using AdvSearcher.Avito.Parser.Steps.FourthStep;
using AdvSearcher.Avito.Parser.Steps.SecondStep;
using AdvSearcher.Avito.Parser.Steps.ThirdStep;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Avito.Parser;

public sealed class AvitoFastParserDependencyInjection : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public AvitoFastParserDependencyInjection(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<IParser, AvitoFastParser>(p =>
            {
                IAvitoFastParserStep step = p.GetRequiredService<IAvitoFastParserStep>();
                AvitoFastParser parser = new AvitoFastParser(step);
                parser.SetMessageListener(_listener);
                return parser;
            })
            .AddScoped<AvitoFastParserPipeline>()
            .AddScoped<IAvitoFastParserStep>(p =>
            {
                AvitoFastParserPipeline pipeline = p.GetRequiredService<AvitoFastParserPipeline>();
                WebDriverProvider driverProvider = p.GetRequiredService<WebDriverProvider>();
                IAvitoFastParserStep fourthStep = new CreateResponsesStep(pipeline, _listener);
                IAvitoFastParserStep thirdStep = new ProcessAdvertisementsStep(
                    pipeline,
                    driverProvider,
                    fourthStep
                );
                IAvitoFastParserStep secondStep = new FetchAvitoCatalogueStep(
                    pipeline,
                    driverProvider,
                    _listener,
                    thirdStep
                );
                IAvitoFastParserStep firstStep = new OpenAvitoCatalogueMainPageStep(
                    pipeline,
                    _listener,
                    driverProvider,
                    secondStep
                );
                return firstStep;
            });
        _listener.Publish("Парсер Авито подгружен");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
