namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Responses;

public readonly record struct PaymentResponse(Guid Id, PaymentStatus Status)
{
    public static PaymentResponse FromPayment(Payment payment) => new(payment.Id, PaymentStatus.Created);
}