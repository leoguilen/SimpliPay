namespace PaymentGateway.Features.Payments.Repositories;

public interface ITransactionsRepository
{
    Task<Result> AddAsync(Payment payment, CancellationToken cancellationToken = default);

    Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Payment>> GetAllAsync(PaymentFilter filter, CancellationToken cancellationToken = default);
}
