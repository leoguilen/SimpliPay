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
}
