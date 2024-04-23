namespace PaymentGateway.Features.Payments.Repositories;

public interface ITransactionsRepository
{
    Task<Result> AddAsync(Payment payment, CancellationToken cancellationToken = default);
}
