namespace PaymentGateway.Enums;

public enum PaymentStatus : byte
{
    /// <summary>
    /// Payment has been created
    /// </summary>
    Created = 0,

    /// <summary>
    /// Payment has invalid format or data
    /// </summary>
    Invalid = 1,

    /// <summary>
    /// Payment is valid and is being processed
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Payment has failed due to an error
    /// </summary>
    Error = 3,

    /// <summary>
    /// The payment was rejected, usually due to a risk control or by the issuing bank
    /// </summary>
    Rejected = 4,

    /// <summary>
    /// Payment has been cancelled
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Payment has been validated and authorized
    /// </summary>
    Authorized = 6,
}
