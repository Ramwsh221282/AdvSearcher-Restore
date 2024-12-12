using AdvSearcher.Core.Tools;

namespace AdvSearcher.Application.Abstractions.CQRS;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query);
}
