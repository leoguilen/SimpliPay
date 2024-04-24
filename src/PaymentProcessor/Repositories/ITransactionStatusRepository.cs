namespace PaymentProcessor.Repositories;

public interface ITransactionStatusRepository
{
    Task SetStatusAsync(
        Guid paymentId,
        PaymentStatus status,
        string? details = null,
        CancellationToken cancellationToken = default);
}
