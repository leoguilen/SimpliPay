namespace PaymentGateway.Test.Features.Payments.Services;

[Trait("Category", "Unit")]
public class PaymentServiceTest
{
    private static readonly PaymentFixture _fixture = new();

    private readonly Mock<IPaymentRiskCheckService> _riskCheckServiceMock;
    private readonly Mock<IDataProtector> _dataProtectorMock;
    private readonly Mock<IDataProtectionProvider> _dataProtectionProviderMock;
    private readonly Mock<ITopicProducer<string, PaymentReceived>> _topicProducerMock;
    private readonly Mock<ITransactionsRepository> _transactionsRepositoryMock;
    private readonly ILogger<PaymentService> _loggerMock;

    public PaymentServiceTest()
    {
        _riskCheckServiceMock = new(MockBehavior.Strict);
        _dataProtectorMock = new(MockBehavior.Strict);
        _dataProtectionProviderMock = new(MockBehavior.Strict);
        _topicProducerMock = new(MockBehavior.Strict);
        _transactionsRepositoryMock = new(MockBehavior.Strict);
        _loggerMock = new NullLogger<PaymentService>();
    }

    [Fact]
    public async Task ExecuteAsync_WhenRiskCheckFails_ShouldReturnFailureResult()
    {
        // Arrange
        var payment = _fixture.CreateSuspiciousPayment();
        var cancellationToken = new CancellationToken();
        _riskCheckServiceMock
            .Setup(x => x.CheckAsync(payment, cancellationToken))
            .ReturnsAsync(Result.Failure("Risk check failed"));
        var sut = GetClassUnderTest();

        // Act
        var result = await sut.ExecuteAsync(payment, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(Result<Payment>.Failure("Risk check failed"));
    }

    [Fact]
    public async Task ExecuteAsync_WhenTransactionRepositoryFails_ShouldReturnFailureResult()
    {
        // Arrange
        var payment = _fixture.CreateValidPayment();
        var cancellationToken = new CancellationToken();
        _riskCheckServiceMock
            .Setup(x => x.CheckAsync(payment, cancellationToken))
            .ReturnsAsync(Result.Success());
        _transactionsRepositoryMock
            .Setup(x => x.AddAsync(payment, cancellationToken))
            .ReturnsAsync(Result.Failure(new Exception("Failed to save payment to the database")));
        var sut = GetClassUnderTest();

        // Act
        var result = await sut.ExecuteAsync(payment, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(Result<Payment>.Failure(new Exception("Failed to save payment to the database")));
    }

    [Fact]
    public async Task ExecuteAsync_WhenAllSucceeds_ShouldReturnSuccessResult()
    {
        // Arrange
        var payment = _fixture.CreateValidPayment();
        var cancellationToken = new CancellationToken();
        _riskCheckServiceMock
            .Setup(x => x.CheckAsync(payment, cancellationToken))
            .ReturnsAsync(Result.Success());
        _transactionsRepositoryMock
            .Setup(x => x.AddAsync(payment, cancellationToken))
            .ReturnsAsync(Result.Success());
        _dataProtectionProviderMock
            .Setup(x => x.CreateProtector($"Payment:{payment.Id}"))
            .Returns(_dataProtectorMock.Object);
        _dataProtectorMock
            .Setup(x => x.Protect(It.IsAny<byte[]>()))
            .Returns(Encoding.UTF8.GetBytes("encryptedPayment"));
        _topicProducerMock
            .Setup(x => x.Produce(
                payment.Id.ToString(),
                It.Is<PaymentReceived>(x => x.Data == "encryptedPayment"),
                cancellationToken))
            .Returns(Task.CompletedTask);
        var sut = GetClassUnderTest();

        // Act
        var result = await sut.ExecuteAsync(payment, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(Result<Payment>.Success(payment));
    }

    private PaymentService GetClassUnderTest() => new(
        _riskCheckServiceMock.Object,
        _dataProtectionProviderMock.Object,
        _topicProducerMock.Object,
        _transactionsRepositoryMock.Object,
        _loggerMock);
}
