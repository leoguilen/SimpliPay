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
}
