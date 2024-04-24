namespace PaymentProcessor.Models;

public record Client
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
