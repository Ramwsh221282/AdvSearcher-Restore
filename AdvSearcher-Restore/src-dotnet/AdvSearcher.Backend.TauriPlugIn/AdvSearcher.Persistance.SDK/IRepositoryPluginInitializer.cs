using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public interface IRepositoryPluginInitializer
{
    public IServiceCollection Modify(IServiceCollection services);
}
