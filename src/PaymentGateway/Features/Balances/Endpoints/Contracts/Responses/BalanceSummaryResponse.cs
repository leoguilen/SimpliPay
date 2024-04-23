namespace PaymentGateway.Features.Balances.Endpoints.Contracts.Responses;

public record BalanceSummaryResponse(ClientResponse Client, BalanceResponse[] Balances)
{
    public static BalanceSummaryResponse From(BalanceSummary summary)
    {
        return new(
            new(summary.Client.Id, summary.Client.Name),
            summary.Balances.Select(b => new BalanceResponse(b.Status.ToString(), b.Amount, b.Transactions.Length)).ToArray()
        );
    }
}
