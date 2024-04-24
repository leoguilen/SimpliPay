namespace PaymentProcessor.Repositories.Impl;

internal sealed class PayablesRepository(IDbConnection dbConnection) : IPayablesRepository
{
    private const string InsertPayableSql = @"
        INSERT INTO payments.payables VALUES (
            @Id,
            @TransactionId,
            @ClientId,
            @Amount,
            @Currency,
            @Status,
            @PaymentDate
        );
    ";

    public async Task AddAsync(
        Payable payable,
        CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: InsertPayableSql,
            parameters: payable,
            cancellationToken: cancellationToken);

        await dbConnection.ExecuteAsync(query);
    }
}
