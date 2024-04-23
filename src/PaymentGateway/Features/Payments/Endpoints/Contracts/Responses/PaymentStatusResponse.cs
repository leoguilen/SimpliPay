namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Responses;

public readonly record struct PaymentStatusResponse(Guid Id, PaymentStatus Status)
{
    public static PaymentStatusResponse FromPayment(Payment payment) => new(payment.Id, PaymentStatus.Created);
}