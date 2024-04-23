namespace PaymentGateway.Test.Features.Payments.Services;

[Trait("Category", "Unit")]
public class PaymentRiskCheckServiceTest
{
    private static readonly Fixture _fixture = new();

    [Fact]
    public async Task CheckAsync_ShouldReturnSuccess_WhenPaymentMethodIsCreditCardAndAmountIsGreaterThanOne()
    {
        // Arrange
        var payment = _fixture.Build<Payment>()
            .Without(p => p.Card)
            .With(p => p.PaymentMethod, PaymentMethod.CreditCard)
            .With(p => p.Amount, 1.1M)
            .Create();
        var sut = new PaymentRiskCheckService();

        // Act
        var result = await sut.CheckAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CheckAsync_ShouldReturnFailure_WhenPaymentMethodIsCreditCardAndAmountIsLessThanOne()
    {
        // Arrange
        var payment = _fixture.Build<Payment>()
            .Without(p => p.Card)
            .With(p => p.PaymentMethod, PaymentMethod.CreditCard)
            .With(p => p.Amount, 0.9M)
            .Create();
        var sut = new PaymentRiskCheckService();

        // Act
        var result = await sut.CheckAsync(payment);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CheckAsync_ShouldReturnSuccess_WhenPaymentMethodIsDebitCardAndAmountIsLessThanTenThousand()
    {
        // Arrange
        var payment = _fixture.Build<Payment>()
            .Without(p => p.Card)
            .With(p => p.PaymentMethod, PaymentMethod.DebitCard)
            .With(p => p.Amount, 9_999M)
            .Create();
        var sut = new PaymentRiskCheckService();

        // Act
        var result = await sut.CheckAsync(payment);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CheckAsync_ShouldReturnFailure_WhenPaymentMethodIsDebitCardAndAmountIsGreaterThanTenThousand()
    {
        // Arrange
        var payment = _fixture.Build<Payment>()
            .Without(p => p.Card)
            .With(p => p.PaymentMethod, PaymentMethod.DebitCard)
            .With(p => p.Amount, 10_001M)
            .Create();
        var sut = new PaymentRiskCheckService();

        // Act
        var result = await sut.CheckAsync(payment);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}