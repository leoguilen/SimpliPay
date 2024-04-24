namespace PaymentProcessor.Services;

internal sealed class PaymentService(IEnumerable<IPaymentProcessor> processors) : IPaymentService
{
    private readonly Dictionary<PaymentMethod, IPaymentProcessor> _processors = new()
    {
        [PaymentMethod.CreditCard] = processors.First(),
        [PaymentMethod.DebitCard] = processors.Last()
    };

    public async Task ExecuteAsync(
        PaymentReceivedEvent payment,
        CancellationToken cancellationToken = default)
    {
        await _processors[payment.PaymentMethod].ProcessAsync(payment, cancellationToken);
    }
}
