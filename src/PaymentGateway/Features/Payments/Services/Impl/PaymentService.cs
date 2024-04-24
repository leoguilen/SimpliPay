using Client = PaymentGateway.Features.Payments.Models.Client;

namespace PaymentGateway.Features.Payments.Services.Impl;

internal sealed class PaymentService(
    IClientContext clientContext,
    IPaymentRiskCheckService riskCheckService,
    ITopicProducer<string, PaymentReceivedEvent> topicProducer,
    ITransactionsRepository transactionsRepository,
    ILogger<PaymentService> logger)
    : IPaymentService
{
    public async Task<Result<Payment>> ExecuteAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        var riskCheckResult = await riskCheckService.CheckAsync(payment, cancellationToken);
        if (!riskCheckResult.IsSuccess)
        {
            logger.LogError("Risk check failed for payment {PaymentId}", payment.Id);
            return Result<Payment>.Failure(riskCheckResult.ErrorMessage!);
        }

        ObuscateCardNumber(ref payment);
        payment = payment with
        {
            Client = new Client()
            {
                Id = clientContext.ClientId,
                Name = string.Empty,
            }
        };

        var result = await transactionsRepository.AddAsync(payment, cancellationToken);
        if (!result.IsSuccess)
        {
            logger.LogError(result.Exception, "Failed to save payment {PaymentId} to the database", payment.Id);
            return Result<Payment>.Failure(result.Exception!);
        }

        await topicProducer.Produce(
            key: payment.Id.ToString(),
            value: new PaymentReceivedEvent(payment),
            cancellationToken);

        logger.LogInformation("Payment {PaymentId} has been successfully transmitted", payment.Id);

        return Result<Payment>.Success(payment);
    }

    private static void ObuscateCardNumber(ref Payment payment)
    {
        payment = payment with
        {
            Card = payment.Card with
            {
                Number = new string('*', 12) + payment.Card.Number[^4..]
            },
        };
    }
}
