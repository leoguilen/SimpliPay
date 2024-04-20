namespace PaymentGateway.Endpoints.Payments.Contracts.Requests;

public record PaymentRequest(
    AmountRequest Amount,
    CardRequest Card,
    PaymentMethod PaymentMethod,
    string Description,
    DateTimeOffset Date
);