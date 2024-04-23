namespace PaymentGateway.Features.Balances.Endpoints;

public static class BalancesEndpoint
{
    public static void MapBalancesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/balances", async (
            [FromServices] IBalanceSummaryService balanceSummaryService,
            CancellationToken cancellationToken) =>
            {
                var summary = await balanceSummaryService.GetSummaryAsync(cancellationToken);
                return Results.Ok(BalanceSummaryResponse.From(summary));
            })
            .Produces<BalanceSummaryResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .WithName("GetBalances")
            .WithSummary("Get balances")
            .WithTags("Balances")
            .WithOpenApi();
    }
}
