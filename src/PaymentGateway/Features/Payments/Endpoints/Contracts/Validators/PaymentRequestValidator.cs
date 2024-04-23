namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Validators;

internal sealed class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.Amount).SetValidator(new AmountRequestValidator()).WithMessage("Amount is not valid");
        RuleFor(x => x.Card).SetValidator(new CardRequestValidator()).WithMessage("Card is not valid");
        RuleFor(x => x.PaymentMethod).IsInEnum().WithMessage("Payment method is not supported");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("Date cannot be in the future");
    }
}
