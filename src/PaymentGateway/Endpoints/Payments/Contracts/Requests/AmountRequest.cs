namespace PaymentGateway.Endpoints.Payments.Contracts.Requests;

public record AmountRequest(
    decimal Value,
    string Currency
);
