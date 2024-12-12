using AdvSearcher.Core.Tools;
using Microsoft.Extensions.Logging;

namespace AdvSearcher.Application.Abstractions.CQRS;

public abstract class BaseValidator(ILogger logger)
{
    private readonly List<Error> _errors = [];

    public void CheckResultForErrors<T>(Result<T> result)
    {
        if (!result.IsFailure)
            return;
        logger.LogInformation("Ошибка: {text}", result.Error.Description);
        _errors.Add(result.Error);
    }

    public bool HasErrors => _errors.Count == 0;
    public Error LastError => _errors.Count == 0 ? Error.None : _errors[^1];
}
