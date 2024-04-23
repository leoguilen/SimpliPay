namespace PaymentGateway.Features.Payments.Services;

public interface IPaymentRiskCheckService
{
    Task<Result> CheckAsync(Payment payment, CancellationToken cancellationToken = default);
}
