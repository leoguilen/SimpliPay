namespace PaymentGateway.Extensions;

[ExcludeFromCodeCoverage]
internal static class ApplicationBuilderExtensions
{
    public static void UseDevelopmentMiddlewares(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(op => op.DocumentTitle = "Payment Gateway API - Swagger");
        app.UseDeveloperExceptionPage();
    }

    public static void UseHealthChecksProbes(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/live", new HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("live"),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.UseHealthChecks("/ready", new HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("ready"),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });
    }
}
