namespace PaymentProcessor.Services;

internal sealed class CreditCardPaymentProcessor(
    IIssuingBankService issuingBankService,
    IClientPaymentMethodsRepository clientPaymentMethodsRepository,
    IPayablesRepository payablesRepository)
    : IPaymentProcessor
{
    public async Task ProcessAsync(
        PaymentReceivedEvent payment,
        CancellationToken cancellationToken = default)
    {
        var (accepted, message) = await issuingBankService
            .ChargeAsync(payment.Card!, payment.Amount, cancellationToken);
        if (!accepted)
        {
            throw new PaymentRejectedException(message);
        }

        var processingFee = await clientPaymentMethodsRepository
            .GetPaymentMethodFeeAsync(payment.Client!.Id, PaymentMethod.CreditCard, cancellationToken);

        var payable = new Payable
        {
            Id = Guid.NewGuid(),
            TransactionId = payment.Id,
            ClientId = payment.Client!.Id,
            Amount = payment.Amount - (payment.Amount * processingFee),
            Currency = payment.Currency!,
            Status = PayableStatus.WaitingFunds,
            PaymentDate = DateTime.Today.AddDays(30),
        };

        await payablesRepository.AddAsync(payable, cancellationToken);
    }
}
