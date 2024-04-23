namespace PaymentGateway.Integration.Test.Fixtures;

internal class PaymentRequestFixture : Fixture
{
    private static readonly Faker _faker = new();

    public PaymentRequestFixture()
    {
        Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
    }

    public PaymentRequest CreateValidPaymentRequest()
    {
        return Build<PaymentRequest>()
            .With(x => x.Amount, new AmountRequest(
                _faker.Finance.Amount(min: 1),
                _faker.Finance.Currency().Code))
            .With(x => x.Card, new CardRequest(
                _faker.Finance.CreditCardNumber(),
                _faker.Person.FullName,
                DateOnly.FromDateTime(_faker.Date.Future()),
                _faker.Finance.CreditCardCvv()))
            .With(x => x.PaymentMethod, _faker.PickRandom<PaymentMethod>())
            .With(x => x.Description, _faker.Lorem.Sentence())
            .With(x => x.Date, DateTimeOffset.UtcNow)
            .Create();
    }

    public PaymentRequest CreateSuspiciousPaymentRequest()
    {
        return CreateValidPaymentRequest() with
        {
            Amount = new AmountRequest(
                _faker.Finance.Amount(max: 100_000M),
                _faker.Finance.Currency().Code),
            PaymentMethod = PaymentMethod.DebitCard,
        };
    }
}
