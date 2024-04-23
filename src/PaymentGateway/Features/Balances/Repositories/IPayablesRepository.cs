namespace PaymentGateway.Features.Balances.Repositories;

public interface IPayablesRepository
{
    Task<IEnumerable<Payable>> GetAllAsync(CancellationToken cancellationToken = default);
}
