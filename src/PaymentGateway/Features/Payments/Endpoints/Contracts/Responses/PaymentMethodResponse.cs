namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Responses;

public record PaymentMethodResponse(string Method, string CardNumber);