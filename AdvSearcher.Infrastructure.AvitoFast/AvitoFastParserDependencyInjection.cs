using AdvSearcher.Infrastructure.AvitoFast.Steps;
using AdvSearcher.Infrastructure.AvitoFast.Steps.FirstStep;
using AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep;
using AdvSearcher.Infrastructure.AvitoFast.Steps.SecondStep;
using AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.AvitoFast;

public sealed class AvitoFastParserDependencyInjection : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<IParser, AvitoFastParser>()
            .AddTransient<AvitoFastParserPipeline>()
            .AddTransient<IAvitoFastParserStep>(p =>
            {
                AvitoFastParserPipeline pipeline = p.GetRequiredService<AvitoFastParserPipeline>();
                WebDriverProvider driverProvider = p.GetRequiredService<WebDriverProvider>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();
                IAvitoFastParserStep fourthStep = new CreateResponsesStep(pipeline, logger);
                IAvitoFastParserStep thirdStep = new ProcessAdvertisementsStep(
                    pipeline,
                    logger,
                    driverProvider,
                    fourthStep
                );
                IAvitoFastParserStep secondStep = new FetchAvitoCatalogueStep(
                    pipeline,
                    driverProvider,
                    logger,
                    thirdStep
                );
                IAvitoFastParserStep firstStep = new OpenAvitoCatalogueMainPageStep(
                    pipeline,
                    logger,
                    driverProvider,
                    secondStep
                );
                return firstStep;
            });
        return services;
    }
}
