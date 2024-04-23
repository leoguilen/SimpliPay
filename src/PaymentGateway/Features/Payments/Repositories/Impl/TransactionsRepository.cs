namespace PaymentGateway.Features.Payments.Repositories.Impl;

internal sealed class TransactionsRepository(
    IClientContext clientContext,
    IDbConnection connection)
    : ITransactionsRepository
{
    public async Task<Result> AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CommandDefinition(
                commandText: SqlStatements.InsertTransaction,
                parameters: new
                {
                    payment.Id,
                    clientContext.ClientId,
                    payment.PaymentMethod,
                    payment.Amount,
                    payment.Currency,
                    payment.Description,
                    CardNumber = payment.Card.Number,
                    CardHolder = payment.Card.HolderName,
                    CardExpiry = payment.Card.ExpiryDate.ToDateTime(TimeOnly.MinValue),
                    CardCvv = payment.Card.Cvv,
                    Status = PaymentStatus.Created,
                    payment.Date
                },
                cancellationToken: cancellationToken);

            var affectedRows = await connection.ExecuteAsync(command);

            return affectedRows == 0
                ? Result.Failure("No rows were affected when saving payment to the database")
                : Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(
        PaymentFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: SqlStatements.GetTransactions,
            parameters: new
            {
                filter.Status,
                From = filter.From ?? DateTime.Today.AddDays(-1),
                To = filter.To ?? DateTime.Today.AddDays(1).AddSeconds(-1),
                clientContext.ClientId
            },
            cancellationToken: cancellationToken
        );

        var resultSet = await connection.QueryAsync<PaymentResultSet>(query);

        return !resultSet.Any()
            ? []
            : resultSet.Select(x => x.ToPayment()!);
    }

    public async Task<Payment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        var query = new CommandDefinition(
            commandText: SqlStatements.GetTransactionById,
            parameters: new { paymentId, clientContext.ClientId },
            cancellationToken: cancellationToken
        );

        var resultSet = await connection.QueryFirstOrDefaultAsync<PaymentResultSet?>(query);

        return resultSet?.ToPayment();
    }
}
