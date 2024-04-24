namespace PaymentProcessor.Repositories;

public interface IClientPaymentMethodsRepository
{
    Task<decimal> GetPaymentMethodFeeAsync(
        Guid clientId,
        PaymentMethod paymentMethod,
        CancellationToken cancellationToken = default);
}
