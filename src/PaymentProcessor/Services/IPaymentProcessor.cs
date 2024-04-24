namespace PaymentProcessor.Services;

public interface IPaymentProcessor
{
    Task ProcessAsync(PaymentReceivedEvent payment, CancellationToken cancellationToken = default);
}
