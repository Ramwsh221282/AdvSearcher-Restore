namespace AdvSearcher.Core.Tools;

public record Error(string Description)
{
    public static readonly Error None = new Error(string.Empty);
}

public sealed class Result<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure { get; init; }
    public T Value { get; init; } = default!;
    public Error Error { get; init; }

    private Result(Error error)
    {
        IsFailure = true;
        Error = error;
    }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        Error = Error.None;
    }

    public static Result<T> Success(T value) => new Result<T>(value);

    public static Result<T> Failure(Error error) => new Result<T>(error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Failure(error);

    public static implicit operator T(Result<T> result) => result.Value;
}
