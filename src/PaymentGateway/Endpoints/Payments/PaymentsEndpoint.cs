namespace PaymentGateway.Endpoints.Payments;

public static class PaymentsEndpoint
{
    public static void MapPaymentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/payments", (
            [FromBody] PaymentRequest request,
            [FromServices] IValidator<PaymentRequest> validator,
            CancellationToken cancellationToken) =>
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return Results.Accepted();
            })
            .WithName("CreatePayment")
            .WithTags("Payments")
            .WithOpenApi();

        // Get payment by id

        // Cancel payment by id

        // Refund payment by id

        // Get refund by id
    }
}
