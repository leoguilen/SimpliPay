namespace PaymentGateway.Features.Balances.Models;

public record Balance
{
    public required PayableStatus Status { get; init; }

    public required decimal Amount { get; init; }

    public required Transaction[] Transactions { get; init; }
}
