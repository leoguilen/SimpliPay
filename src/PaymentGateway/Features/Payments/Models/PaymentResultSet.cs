
namespace PaymentGateway.Features.Payments.Models;

internal record PaymentResultSet
{
    public Guid Id { get; init; }

    public Guid ClientId { get; init; }

    public string? ClientName { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public decimal Amount { get; init; }

    public string? Currency { get; init; }

    public string? Description { get; init; }

    public string? CardNumber { get; init; }

    public DateTimeOffset Date { get; init; }

    public PaymentStatus Status { get; init; }

    internal Payment? ToPayment()
    {
        return Id == Guid.Empty
            ? null
            : new Payment
            {
                Id = Id,
                Client = new() { Id = ClientId, Name = ClientName! },
                PaymentMethod = PaymentMethod,
                Amount = Amount,
                Currency = Currency!,
                Description = Description!,
                Card = new Card()
                {
                    Number = CardNumber!,
                },
                Date = Date,
                Status = Status
            };
    }
}
