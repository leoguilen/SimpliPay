namespace PaymentGateway.Features.Balances.Services.Impl;

internal sealed class BalanceSummaryService(
    IClientContext clientContext,
    IPayablesRepository payablesRepository) : IBalanceSummaryService
{
    public async Task<BalanceSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var payables = await payablesRepository.GetAllAsync(cancellationToken);
        if (!payables.Any())
        {
            return new BalanceSummary
            {
                Client = new()
                {
                    Id = clientContext.ClientId,
                    Name = string.Empty
                },
                Balances = [
                new Balance
                {
                    Status = PayableStatus.Paid,
                    Amount = 0,
                    Transactions = []
                },
                new Balance
                {
                    Status = PayableStatus.WaitingFunds,
                    Amount = 0,
                    Transactions = []
                },
            ]
            };
        }

        var balances = payables
            .GroupBy(p => p.Status)
            .Select(g => new Balance
            {
                Status = g.Key,
                Amount = g.Sum(p => p.Amount),
                Transactions = g.Select(p => p.Transaction).ToArray()
            })
            .ToArray();

        return new BalanceSummary
        {
            Client = payables.First().Client,
            Balances = balances,
        };
    }
}
