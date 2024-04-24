namespace PaymentProcessor.Repositories.Impl;

internal sealed class TransactionStatusRepository(IDbConnection dbConnection) : ITransactionStatusRepository
{
    private const string InsertStatusSql = @"
        INSERT INTO payments.transaction_status
        VALUES (@PaymentId, @Status, @Details, CURRENT_TIMESTAMP);
    ";

    public async Task SetStatusAsync(
        Guid paymentId,
        PaymentStatus status,
        string? details = null,
        CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            commandText: InsertStatusSql,
            parameters: new { paymentId, status, details },
            cancellationToken: cancellationToken);

        await dbConnection.ExecuteAsync(command);
    }
}
