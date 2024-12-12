using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Abstractions.CQRS;

public interface ICommandValidator<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    bool Validate(TCommand command);
    Error LastError { get; }
}
