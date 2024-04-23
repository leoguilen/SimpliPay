namespace PaymentGateway.Features.Balances.Models;

public record BalanceSummary
{
    public required Client Client { get; init; }

    public required Balance[] Balances { get; init; }
}
