namespace PaymentGateway.Features.Payments.Models;

public record PaymentFilter
{
    public PaymentStatus? Status { get; init; }

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }
}
