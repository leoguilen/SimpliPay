using Client = PaymentGateway.Features.Payments.Models.Client;

namespace PaymentGateway.Features.Payments.Events;

[MessageUrn("simplipay:payment-received")]
public record PaymentReceivedEvent
{
    public PaymentReceivedEvent(Payment payment)
    {
        Id = payment.Id;
        Client = payment.Client;
        Amount = payment.Amount;
        Currency = payment.Currency;
        Card = payment.Card;
        PaymentMethod = payment.PaymentMethod;
        Description = payment.Description;
        Date = payment.Date;
        Status = payment.Status;
    }

    public Guid Id { get; init; }

    public Client? Client { get; init; }

    public decimal Amount { get; init; }

    public string Currency { get; init; }

    public Card Card { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public string Description { get; init; }

    public DateTimeOffset Date { get; init; }

    public PaymentStatus Status { get; init; }
}
