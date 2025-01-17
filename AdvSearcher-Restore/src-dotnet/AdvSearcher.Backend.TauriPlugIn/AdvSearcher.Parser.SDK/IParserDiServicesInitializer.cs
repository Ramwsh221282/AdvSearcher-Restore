using AdvSearcher.Application.Contracts.MessageListeners;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Parser.SDK;

public interface IParserDiServicesInitializer : IListenable
{
    IServiceCollection ModifyServices(IServiceCollection services);
}
