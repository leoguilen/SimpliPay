namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Requests;

public record PaymentRequest(
    AmountRequest Amount,
    CardRequest Card,
    PaymentMethod PaymentMethod,
    string Description,
    DateTimeOffset Date
)
{
    public static implicit operator Payment(PaymentRequest request) => new()
    {
        Id = Guid.NewGuid(),
        Amount = request.Amount.Value,
        Currency = request.Amount.Currency,
        Card = new Card
        {
            Number = request.Card.Number,
            HolderName = request.Card.HolderName,
            ExpiryDate = request.Card.ExpiryDate,
            Cvv = request.Card.Cvv,
        },
        PaymentMethod = request.PaymentMethod,
        Description = request.Description,
        Date = request.Date,
    };
}