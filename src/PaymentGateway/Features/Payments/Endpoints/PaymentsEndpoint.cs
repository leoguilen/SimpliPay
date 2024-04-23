namespace PaymentGateway.Features.Payments.Endpoints;

public static class PaymentsEndpoint
{
    public static void MapPaymentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/payments", async (
            [FromBody] PaymentRequest request,
            [FromServices] IValidator<PaymentRequest> requestValidator,
            [FromServices] IPaymentService paymentService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
            {
                var validationResult = requestValidator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var result = await paymentService.ExecuteAsync(request, cancellationToken);
                if (!result.IsSuccess)
                {
                    var problemDetails = new ProblemDetails()
                    {
                        Title = "Payment request failed",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = result.ErrorMessage,
                        Instance = httpContext.Request.Path,
                        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21",
                    };
                    return Results.UnprocessableEntity(problemDetails);
                }

                return Results.Accepted(
                    uri: $"/api/v1/payments/{result!.Value!.Id}",
                    value: PaymentResponse.FromPayment(result!.Value!));
            })
            .Accepts<PaymentRequest>("application/json")
            .Produces<PaymentResponse>(StatusCodes.Status202Accepted, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
            .WithName("CreatePayment")
            .WithSummary("Create a new payment")
            .WithDescription("Create a new payment")
            .WithTags("Payments")
            .WithOpenApi();

        // Get payment by id

        // Cancel payment by id

        // Refund payment by id

        // Get refund by id
    }
}
