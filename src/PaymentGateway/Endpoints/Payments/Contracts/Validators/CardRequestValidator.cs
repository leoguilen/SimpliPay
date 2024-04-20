namespace PaymentGateway;

internal sealed class CardRequestValidator : AbstractValidator<CardRequest>
{
    public CardRequestValidator()
    {
        RuleFor(x => x.Number).CreditCard().WithMessage("Card number is not valid");
        RuleFor(x => x.HolderName).NotEmpty().WithMessage("Holder name is required");
        RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Expiry date is not valid");
        RuleFor(x => x.Cvv).Length(3).WithMessage("CVV is not valid");
    }
}
