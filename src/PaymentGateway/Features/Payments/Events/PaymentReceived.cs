namespace PaymentGateway;

[MessageUrn("simplipay:payment-received")]
public record PaymentReceived(string Data);
