namespace PaymentProcessor.Events;

[MessageUrn("simplipay:payment-received")]
public record PaymentReceivedEvent
{
    public Guid Id { get; init; }

    public Client? Client { get; init; }

    public decimal Amount { get; init; }

    public string? Currency { get; init; }

    public Card? Card { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public string? Description { get; init; }

    public DateTimeOffset Date { get; init; }

    public PaymentStatus Status { get; init; }
}
