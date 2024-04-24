namespace PaymentProcessor.Repositories.Impl;

internal sealed class ClientPaymentMethodsRepository(IDbConnection dbConnection) : IClientPaymentMethodsRepository
{
    private const string GetPaymentMethodFeeSql = @"
        SELECT fee
        FROM clients.client_payment_methods
        WHERE client_id = @ClientId AND payment_method_id = @PaymentMethod
    ";

    public async Task<decimal> GetPaymentMethodFeeAsync(
        Guid clientId,
        PaymentMethod paymentMethod,
        CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: GetPaymentMethodFeeSql,
            parameters: new { clientId, paymentMethod },
            cancellationToken: cancellationToken);

        return await dbConnection.ExecuteScalarAsync<decimal?>(query) ?? decimal.Zero;
    }
}
