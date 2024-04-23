namespace PaymentGateway.Features.Balances.Models;

public record Payable
{
    public required Guid Id { get; init; }

    public required Transaction Transaction { get; init; }

    public required Client Client { get; init; }

    public required decimal Amount { get; init; }

    public required string Currency { get; init; }

    public required PayableStatus Status { get; init; }

    public required DateTime PaymentDate { get; init; }
}
