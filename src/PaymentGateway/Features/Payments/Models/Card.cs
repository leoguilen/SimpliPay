namespace PaymentGateway.Features.Payments.Models;

public record Card
{
    public string Number { get; init; } = string.Empty;

    public string HolderName { get; init; } = string.Empty;

    public DateOnly ExpiryDate { get; init; }

    public string Cvv { get; init; } = string.Empty;
}
