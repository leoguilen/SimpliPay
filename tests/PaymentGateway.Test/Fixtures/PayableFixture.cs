namespace PaymentGateway.Test;

internal class PayableFixture : Fixture
{
    private static readonly Faker _faker = new();

    public IEnumerable<Payable> Create(int count = 1)
    {
        return Build<Payable>()
            .With(x => x.Id, _faker.Random.Uuid())
            .With(x => x.Amount, _faker.Finance.Amount(min: 1))
            .With(x => x.Status, _faker.PickRandom<PayableStatus>())
            .With(x => x.Transaction, new Transaction
            {
                Id = _faker.Random.Uuid(),
                Description = _faker.Lorem.Sentence(),
                Amount = _faker.Finance.Amount(min: 1),
                Date = _faker.Date.Past()
            })
            .CreateMany(count);
    }
}
