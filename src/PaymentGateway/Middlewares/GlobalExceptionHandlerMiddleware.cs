namespace PaymentGateway.Middlewares;

internal sealed class GlobalExceptionHandlerMiddleware(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred while processing the request");

        var problemDetailsContext = new ProblemDetailsContext()
        {
            ProblemDetails = new ProblemDetails()
            {
                Title = "An error occurred while processing the request",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            },
            HttpContext = httpContext,
            Exception = exception,
            AdditionalMetadata = new(new Dictionary<string, object>
            {
                ["RequestId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["CorrelationId"] = Guid.Empty,
            }),
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
