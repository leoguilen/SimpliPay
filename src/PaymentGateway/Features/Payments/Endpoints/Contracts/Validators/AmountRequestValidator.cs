namespace PaymentGateway.Features.Payments.Endpoints.Contracts.Validators;

internal sealed class AmountRequestValidator : AbstractValidator<AmountRequest>
{
    public AmountRequestValidator()
    {
        RuleFor(x => x.Value).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Currency).Length(3).WithMessage("Currency is not valid");
    }
}
