namespace PaymentGateway.Endpoints.Payments.Contracts.Requests;

public record CardRequest(
    string Number,
    string HolderName,
    DateOnly ExpiryDate,
    string Cvv
);
