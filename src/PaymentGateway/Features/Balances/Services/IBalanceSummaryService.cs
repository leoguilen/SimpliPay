namespace PaymentGateway.Features.Balances.Services;

public interface IBalanceSummaryService
{
    Task<BalanceSummary> GetSummaryAsync(CancellationToken cancellationToken = default);
}
