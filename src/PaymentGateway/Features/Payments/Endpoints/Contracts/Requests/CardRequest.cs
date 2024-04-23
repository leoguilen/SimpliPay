namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Requests;

public record CardRequest(
    string Number,
    string HolderName,
    DateOnly ExpiryDate,
    string Cvv
);
