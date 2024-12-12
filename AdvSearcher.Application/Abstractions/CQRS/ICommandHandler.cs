using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Abstractions.CQRS;

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(TCommand command);
}
