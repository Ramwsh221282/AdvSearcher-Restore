using AdvSearcher.Core.Entities.ServiceUrls.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

public sealed record ServiceUrlMode
{
    public static readonly ServiceUrlMode Loadable = new("Загрузка");
    public static readonly ServiceUrlMode Publicatable = new("Публикация");

    public string Mode { get; init; }

    private ServiceUrlMode()
    {
        Mode = string.Empty;
    } // ef core constructor

    private ServiceUrlMode(string mode) => Mode = mode;

    public static Result<ServiceUrlMode> Create(string? mode)
    {
        if (string.IsNullOrWhiteSpace(mode))
            return ServiceUrlsErrors.ModeEmpty;
        if (!string.Equals(mode, Loadable.Mode) || !string.Equals(mode, Publicatable.Mode))
            return ServiceUrlsErrors.InvalidMode;
        return new ServiceUrlMode(mode);
    }
}
