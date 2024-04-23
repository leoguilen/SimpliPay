namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Responses;

public record PaymentResponse(
    Guid Id,
    ClientResponse Client,
    AmountResponse Amount,
    PaymentMethodResponse PaymentMethod,
    string Description,
    DateTimeOffset Date,
    string Status
)
{
    public static PaymentResponse FromPayment(Payment payment) => new(
        payment.Id,
        new(payment.Client!.Id, payment.Client!.Name),
        new(payment.Amount, payment.Currency),
        new(payment.PaymentMethod.ToString(), payment.Card.Number),
        payment.Description,
        payment.Date,
        payment.Status.ToString());
}
