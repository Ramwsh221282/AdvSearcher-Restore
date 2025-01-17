using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public sealed record ServiceUrlMode
{
    public static readonly ServiceUrlMode Loadable = new("Загрузка");
    public static readonly ServiceUrlMode Publicatable = new("Публикация");

    public string Mode { get; init; } = string.Empty;

    private ServiceUrlMode() { } // EF Core Constructor;

    private ServiceUrlMode(string mode) => Mode = mode;

    public static Result<ServiceUrlMode> Create(string? mode) =>
        mode switch
        {
            null => ServiceUrlsErrors.ModeEmpty,
            not null when string.IsNullOrWhiteSpace(mode) => ServiceUrlsErrors.ModeEmpty,
            not null
                when !string.Equals(mode, Loadable.Mode)
                    && !string.Equals(mode, Publicatable.Mode) => ServiceUrlsErrors.InvalidMode,
            _ => new ServiceUrlMode(mode),
        };
}
