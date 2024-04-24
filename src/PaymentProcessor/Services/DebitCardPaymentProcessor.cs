namespace PaymentProcessor.Services;

internal sealed class DebitCardPaymentProcessor(
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
            .GetPaymentMethodFeeAsync(payment.Client!.Id, PaymentMethod.DebitCard, cancellationToken);

        var payable = new Payable
        {
            Id = Guid.NewGuid(),
            TransactionId = payment.Id,
            ClientId = payment.Client!.Id,
            Amount = payment.Amount - (payment.Amount * processingFee),
            Currency = payment.Currency!,
            Status = PayableStatus.Paid,
            PaymentDate = DateTime.Today,
        };

        await payablesRepository.AddAsync(payable, cancellationToken);
    }
}
