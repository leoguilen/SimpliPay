namespace PaymentGateway.Features.Payments.Models;

public record Result(bool IsSuccess, string? ErrorMessage, Exception? Exception = null)
{
    public static Result Success() => new(true, null);

    public static Result Failure(string errorMessage) => new(false, errorMessage);

    public static Result Failure(Exception exception) => new(false, exception.Message, exception);

    public static implicit operator bool(Result result) => result.IsSuccess;
}

public record Result<T>(bool IsSuccess, string? ErrorMessage, T? Value)
{
    public static Result<T> Success(T value) => new(true, null, value);

    public static Result<T> Failure(string errorMessage) => new(false, errorMessage, default);

    public static implicit operator bool(Result<T> result) => result.IsSuccess;
}
