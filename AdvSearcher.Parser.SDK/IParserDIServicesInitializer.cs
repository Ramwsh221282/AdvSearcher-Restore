using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK;

public interface IParserDIServicesInitializer
{
    IServiceCollection ModifyServices(IServiceCollection services);
}
