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
                    return Results.Problem(result.ToProblemDetails(httpContext));
                }

                return Results.Accepted(
                    uri: $"/api/v1/payments/{result!.Value!.Id}",
                    value: PaymentStatusResponse.FromPayment(result!.Value!));
            })
            .Accepts<PaymentRequest>("application/json")
            .Produces<PaymentStatusResponse>(StatusCodes.Status202Accepted, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
            .WithName("CreatePayment")
            .WithSummary("Create a new payment")
            .WithTags("Payments")
            .WithOpenApi();

        endpoints.MapGet("/payments/{id}", async (
            [FromRoute] Guid id,
            [FromServices] ITransactionsRepository repository,
            CancellationToken cancellationToken) =>
            {
                var payment = await repository.GetByIdAsync(id, cancellationToken);
                return payment is null
                    ? Results.NotFound()
                    : Results.Ok(PaymentResponse.FromPayment(payment));
            })
            .Produces<PaymentResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
            .WithName("GetPayment")
            .WithSummary("Get payment details")
            .WithTags("Payments")
            .WithOpenApi();

        endpoints.MapGet("/payments", async (
            [FromQuery] PaymentStatus? status,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromServices] ITransactionsRepository repository,
            CancellationToken cancellationToken) =>
            {
                var filter = new PaymentFilter
                {
                    Status = status,
                    From = from,
                    To = to
                };

                var payments = await repository.GetAllAsync(filter, cancellationToken);

                return Results.Ok(payments.Select(PaymentResponse.FromPayment));
            })
            .Produces<IEnumerable<PaymentResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
            .WithName("GetPayments")
            .WithSummary("Get all payments")
            .WithTags("Payments")
            .WithOpenApi();

        // Get client balance (payables)
    }
}
