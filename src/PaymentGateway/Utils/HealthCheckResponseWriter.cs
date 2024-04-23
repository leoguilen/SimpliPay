namespace PaymentGateway.Utils;

internal static class HealthCheckResponseWriter
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static async Task WriteResponse(
        HttpContext context,
        HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            report.Status,
            report.TotalDuration,
            Entries = report.Entries.Select(entry => new
            {
                entry.Key,
                Value = entry.Value.Status.ToString(),
                entry.Value.Duration,
            }),
        };

        await context.Response.WriteAsJsonAsync(response, s_jsonOptions, context.RequestAborted);
    }
}
