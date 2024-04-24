namespace PaymentProcessor.Repositories;

public interface IPayablesRepository
{
    Task AddAsync(Payable payable, CancellationToken cancellationToken = default);
}