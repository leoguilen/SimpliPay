namespace PaymentGateway.Test.Fixtures;

internal class PaymentFixture : Fixture
{
    private static readonly Faker _faker = new();

    public PaymentFixture()
    {
        Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
    }

    public Payment CreateValidPayment()
    {
        return Build<Payment>()
            .With(x => x.Id, _faker.Random.Uuid())
            .With(x => x.Amount, _faker.Finance.Amount(min: 1))
            .With(x => x.Currency, _faker.Finance.Currency().Code)
            .With(x => x.Card, new Card()
            {
                Number = _faker.Finance.CreditCardNumberObfuscated(separator: ""),
                HolderName = _faker.Person.FullName,
                ExpiryDate = DateOnly.FromDateTime(_faker.Date.Future().Date),
                Cvv = _faker.Finance.CreditCardCvv(),
            })
            .With(x => x.PaymentMethod, _faker.PickRandom<PaymentMethod>())
            .With(x => x.Description, _faker.Lorem.Sentence())
            .With(x => x.Date, DateTimeOffset.UtcNow)
            .Create();
    }

    public Payment CreateSuspiciousPayment()
    {
        return CreateValidPayment() with
        {
            Amount = _faker.Finance.Amount(max: 100_000M),
            PaymentMethod = PaymentMethod.DebitCard,
        };
    }
}
