namespace PaymentGateway.Features.Balances.Models;

public record Transaction
{
    public Guid Id { get; init; }

    public string? Description { get; init; }

    public decimal Amount { get; init; }

    public DateTime Date { get; init; }
}
