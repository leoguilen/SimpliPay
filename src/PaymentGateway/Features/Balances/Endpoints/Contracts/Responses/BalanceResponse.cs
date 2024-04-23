namespace PaymentGateway.Features.Balances.Endpoints.Contracts.Responses;

public record BalanceResponse(string Status, decimal Amount, int TransactionsCount);
