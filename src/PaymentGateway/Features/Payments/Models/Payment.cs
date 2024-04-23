namespace PaymentGateway.Features.Payments.Models;

public record Payment
{
    public required Guid Id { get; init; }

    public required decimal Amount { get; init; }

    public required string Currency { get; init; }

    public required Card Card { get; init; }

    public required PaymentMethod PaymentMethod { get; init; }

    public required string Description { get; init; }

    public required DateTimeOffset Date { get; init; }

    public PaymentStatus Status { get; init; }

    public Client? Client { get; init; }
}
