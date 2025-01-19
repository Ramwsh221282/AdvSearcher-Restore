using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Image.Inpainting.SDK;

public static class InpaintingSdkLoader
{
    private static readonly string Path = $@"{Environment.CurrentDirectory}\Plugins\Inpainting";

    public static IServiceCollection LoadInpainting(this IServiceCollection services)
    {
        Assembly[] assemblies = Directory
            .GetFiles(Path, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();

        services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IInpaintingProcessor>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );

        IServiceProvider provider = services.BuildServiceProvider();
        if (!provider.GetServices<IInpaintingProcessor>().Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR. Inpainting services were not loaded");
            throw new ApplicationException();
        }

        return services;
    }
}
