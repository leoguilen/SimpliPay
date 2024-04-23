namespace PaymentGateway.Features.Balances.Repositories.Impl;

internal sealed class PayablesRepository(
    IClientContext clientContext,
    IDbConnection connection)
    : IPayablesRepository
{
    public async Task<IEnumerable<Payable>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: SqlStatements.GetPayables,
            parameters: new { clientContext.ClientId },
            cancellationToken: cancellationToken);

        var resultSet = await connection.QueryAsync<PayableResultSet>(query);

        return resultSet.Select(x => x.ToPayable());
    }
}
