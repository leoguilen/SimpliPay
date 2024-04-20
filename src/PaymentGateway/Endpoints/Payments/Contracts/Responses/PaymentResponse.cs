namespace PaymentGateway;

public readonly record struct PaymentResponse(Guid Id, PaymentStatus Status);