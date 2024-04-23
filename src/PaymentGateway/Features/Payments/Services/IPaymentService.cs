namespace PaymentGateway.Features.Payments.Services;

public interface IPaymentService
{
    Task<Result<Payment>> ExecuteAsync(Payment payment, CancellationToken cancellationToken = default);
}
