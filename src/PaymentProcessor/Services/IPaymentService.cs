namespace PaymentProcessor.Services;

public interface IPaymentService
{
    Task ExecuteAsync(PaymentReceivedEvent payment, CancellationToken cancellationToken = default);
}
