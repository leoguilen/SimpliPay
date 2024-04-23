namespace PaymentGateway.Test.Features.Balances.Services;

[Trait("Category", "Unit")]
public class BalanceSummaryServiceTest
{
    private static readonly PayableFixture _payableFixture = new();

    [Fact]
    public async Task GetSummaryAsync_ReturnsSummaryWithBalances()
    {
        // Arrange
        var payables = _payableFixture.Create(count: 3);
        var clientContextMock = new Mock<IClientContext>();
        var payablesRepositoryMock = new Mock<IPayablesRepository>();
        clientContextMock
            .Setup(c => c.ClientId)
            .Returns(Guid.NewGuid());
        payablesRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(payables);
        var sut = new BalanceSummaryService(
            clientContextMock.Object,
            payablesRepositoryMock.Object);

        // Act
        var result = await sut.GetSummaryAsync();

        // Assert
        result.Should().Match<BalanceSummary>(summary =>
            summary.Client == payables.First().Client &&
            summary.Balances.Length != 0);
    }
}