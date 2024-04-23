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
}
