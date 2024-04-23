namespace PaymentGateway.Features.Balances.Models;

public record PayableResultSet
{
    public required Guid Id { get; init; }

    public required Guid TransactionId { get; init; }

    public required string TransactionDescription { get; init; }

    public required decimal TransactionAmount { get; init; }

    public required DateTime TransactionDate { get; init; }

    public required Guid ClientId { get; init; }

    public required string ClientName { get; init; }

    public required decimal Amount { get; init; }

    public required string Currency { get; init; }

    public required PayableStatus Status { get; init; }

    public required DateTime PaymentDate { get; init; }

    public Payable ToPayable()
    {
        return new Payable
        {
            Id = Id,
            Transaction = new Transaction
            {
                Id = TransactionId,
                Description = TransactionDescription,
                Amount = TransactionAmount,
                Date = TransactionDate
            },
            Client = new Client
            {
                Id = ClientId,
                Name = ClientName
            },
            Amount = Amount,
            Currency = Currency,
            Status = Status,
            PaymentDate = PaymentDate,
        };
    }
}
