namespace PaymentGateway.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options
                .SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway API",
                    Description = @"
                        A simple payment gateway for SimpliPay.
                        This service is used as an easy integration interface for merchants to process payments and view their accounts receivable.",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "SimpliPay",
                        Email = "support@simplipay.com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    },
                });

            options.SupportNonNullableReferenceTypes();
        });
        return services;
    }

    public static IServiceCollection AddDefaultServices(this IServiceCollection services)
    {
        return services
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandlerMiddleware>()
            .AddHealthChecks()
            .Services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<PaymentRequest>, PaymentRequestValidator>();
    }
}
