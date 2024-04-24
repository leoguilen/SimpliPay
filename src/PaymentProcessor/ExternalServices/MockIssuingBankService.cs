namespace PaymentProcessor.ExternalServices;

internal sealed class MockIssuingBankService : IIssuingBankService
{
    public Task<(bool Accepted, string Message)> ChargeAsync(
        Card card,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        return (card.Number[^1], amount) switch
        {
            ('0', > 5_000) => Task.FromResult((false, "Insufficient funds")),
            ('9', _) => Task.FromResult((false, "Blocked card")),
            _ => Task.FromResult((true, string.Empty)),
        };
    }
}
