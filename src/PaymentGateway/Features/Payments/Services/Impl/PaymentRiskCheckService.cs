namespace PaymentGateway.Features.Payments.Services.Impl;

internal sealed class PaymentRiskCheckService : IPaymentRiskCheckService
{
    public Task<Result> CheckAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        // TODO: Integration with external risk check service

        return payment.PaymentMethod switch
        {
            PaymentMethod.CreditCard => Task.FromResult(CheckCreditCard(payment, cancellationToken)),
            PaymentMethod.DebitCard => Task.FromResult(CheckDebitCard(payment, cancellationToken)),
            _ => Task.FromResult(Result.Failure("Unknown payment method"))
        };
    }

    private static Result CheckDebitCard(Payment payment, CancellationToken cancellationToken)
    {
        return payment.Amount > 10_000
            ? Result.Failure("Debit card payment amount exceeds the limit")
            : Result.Success();
    }

    private static Result CheckCreditCard(Payment payment, CancellationToken cancellationToken)
    {
        return payment.Amount > 0.1M && payment.Amount < 1M
            ? Result.Failure("Credit card payment amount is too low")
            : Result.Success();
    }
}
