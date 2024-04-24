namespace PaymentProcessor.Exceptions;

public class PaymentRejectedException(string reason) : Exception
{
    public string Reason { get; } = reason;
}
