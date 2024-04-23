namespace PaymentGateway;

internal static class ResultExtensions
{
    public static ProblemDetails ToProblemDetails<T>(this Result<T> result, HttpContext httpContext)
    {
        return new ProblemDetails()
        {
            Title = "Payment request failed",
            Status = result.Exception is null
                ? StatusCodes.Status422UnprocessableEntity
                : StatusCodes.Status500InternalServerError,
            Detail = result.ErrorMessage,
            Instance = httpContext.Request.Path,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21",
        };
    }
}
