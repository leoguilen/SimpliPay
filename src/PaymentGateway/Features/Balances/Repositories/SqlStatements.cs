namespace PaymentGateway.Features.Balances.Repositories;

internal static class SqlStatements
{
    internal const string GetPayables = @"
        SELECT
            p.id AS ""Id"",
            t.id AS ""TransactionId"",
            t.description AS ""TransactionDescription"",
            t.amount AS ""TransactionAmount"",
            t.date AS ""TransactionDate"",
            c.id AS ""ClientId"",
            c.name AS ""ClientName"",
            p.amount AS ""Amount"",
            p.currency AS ""Currency"",
            p.status AS ""Status"",
            p.payment_date AS ""PaymentDate""
        FROM payments.payables p
        JOIN payments.transactions t ON t.id = p.transaction_id
        JOIN clients.clients c ON c.id = p.client_id
        WHERE p.client_id = @ClientId
        ORDER BY p.creation_date DESC;
    ";
}
