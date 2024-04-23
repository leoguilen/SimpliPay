namespace PaymentGateway.Features.Balances.Models;

public record Client
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
