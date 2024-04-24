namespace PaymentProcessor.ExternalServices;

public interface IIssuingBankService
{
    Task<(bool Accepted, string Message)> ChargeAsync(Card card, decimal amount, CancellationToken cancellationToken = default);
}
