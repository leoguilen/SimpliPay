namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Requests;

public record AmountRequest(
    decimal Value,
    string Currency
);
