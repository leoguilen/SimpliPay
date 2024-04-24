namespace PaymentProcessor.Consumers;

internal sealed class PaymentReceivedEventConsumer(
    ITransactionStatusRepository transactionStatusRepository,
    IPaymentService paymentService,
    ILogger<PaymentReceivedEventConsumer> logger)
    : IConsumer<PaymentReceivedEvent>
{
    public async Task Consume(ConsumeContext<PaymentReceivedEvent> context)
    {
        await transactionStatusRepository.SetStatusAsync(
            context.Message.Id,
            PaymentStatus.Processing,
            cancellationToken: context.CancellationToken);

        try
        {
            await paymentService.ExecuteAsync(context.Message, context.CancellationToken);

            await transactionStatusRepository.SetStatusAsync(
                context.Message.Id,
                PaymentStatus.Authorized,
                cancellationToken: context.CancellationToken);

            logger.LogInformation("Processed payment {PaymentId}", context.Message.Id);
        }
        catch (PaymentRejectedException ex)
        {
            await transactionStatusRepository.SetStatusAsync(
                context.Message.Id,
                PaymentStatus.Rejected,
                ex.Reason,
                context.CancellationToken);

            logger.LogWarning("Rejected payment {PaymentId}: {Reason}", context.Message.Id, ex.Reason);
        }
        catch (Exception ex)
        {
            await transactionStatusRepository.SetStatusAsync(
                context.Message.Id,
                PaymentStatus.Error,
                ex.Message,
                context.CancellationToken);

            logger.LogError(ex, "Failed to process payment {PaymentId}", context.Message.Id);
        }
    }
}
