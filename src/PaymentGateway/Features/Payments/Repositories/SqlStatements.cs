namespace PaymentGateway.Features.Payments.Repositories;

internal static class SqlStatements
{
    public const string InsertTransaction = @"
        WITH added_transaction AS (
            INSERT INTO payments.transactions (
                id,
                client_id,
                payment_method_id,
                amount,
                currency,
                description,
                card_number,
                card_holder,
                card_expiry,
                cvv,
                date
            )
            VALUES (
                @Id,
                @ClientId,
                @PaymentMethod,
                @Amount,
                @Currency,
                @Description,
                @CardNumber,
                @CardHolder,
                @CardExpiry,
                @CardCvv,
                @Date
            )
            RETURNING id
        )
        INSERT INTO payments.transaction_status(
            transaction_id,
            status,
            details,
            timestamp
        )
        VALUES(
            (SELECT id FROM added_transaction),
            0,
            null,
            CURRENT_TIMESTAMP
        );
    ";
    public static string GetTransactions = @"
        SELECT
            t.id AS ""Id"",
            c.id AS ""ClientId"",
            c.name AS ""ClientName"",
            pm.id AS ""PaymentMethod"",
            t.amount AS ""Amount"",
            t.currency AS ""Currency"",
            t.description AS ""Description"",
            t.card_number AS ""CardNumber"",
            t.date AS ""Date"",
            ts.status AS ""Status""
        FROM payments.transaction_status ts
        JOIN payments.transactions t ON t.id = ts.transaction_id
        JOIN payments.payment_methods pm ON pm.id = t.payment_method_id
        JOIN clients.clients c ON c.id = t.client_id
        WHERE t.client_id = @ClientId
            AND t.date BETWEEN @From AND @To
            AND (@Status is null or ts.status = @Status)
        ORDER BY t.date DESC;
    ";

    public static string GetTransactionById = @"
        SELECT
            t.id AS ""Id"",
            c.id AS ""ClientId"",
            c.name AS ""ClientName"",
            pm.id AS ""PaymentMethod"",
            t.amount AS ""Amount"",
            t.currency AS ""Currency"",
            t.description AS ""Description"",
            t.card_number AS ""CardNumber"",
            t.date AS ""Date"",
            ts.status AS ""Status""
        FROM payments.transaction_status ts
        JOIN payments.transactions t ON t.id = ts.transaction_id
        JOIN payments.payment_methods pm ON pm.id = t.payment_method_id
        JOIN clients.clients c ON c.id = t.client_id
        WHERE ts.transaction_id = @PaymentId AND t.client_id = @ClientId
        ORDER BY ts.timestamp DESC
        LIMIT 1;
    ";
}
