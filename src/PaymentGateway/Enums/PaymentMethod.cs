namespace PaymentGateway.Enums;

public enum PaymentMethod : byte
{
    /// <summary>
    /// Credit card payment method
    /// </summary>
    CreditCard = 1,

    /// <summary>
    /// Debit card payment method
    /// </summary>
    DebitCard = 2,
}
