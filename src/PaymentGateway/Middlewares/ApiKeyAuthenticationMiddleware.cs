namespace PaymentGateway.Middlewares;

internal sealed class ApiKeyAuthenticationMiddleware(IApiKeyValidation apiKeyValidation) : IMiddleware
{
    private const string ApiKeyHeader = "X-Api-Key";

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (!await apiKeyValidation.IsValidAsync(apiKey.ToString(), context.RequestAborted))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }
}
