namespace PaymentProcessor.Test.Consumers;

[Trait("Category", "Unit")]
public class PaymentReceivedEventConsumerTest
{
    private readonly Mock<ITransactionStatusRepository> _transactionStatusRepositoryMock;
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly ILogger<PaymentReceivedEventConsumer> _loggerMock;

    public PaymentReceivedEventConsumerTest()
    {
        _transactionStatusRepositoryMock = new Mock<ITransactionStatusRepository>();
        _paymentServiceMock = new Mock<IPaymentService>();
        _loggerMock = new NullLogger<PaymentReceivedEventConsumer>();
    }

    [Fact]
    public async Task Consume_WithSuccessfulPayment_ShouldSetStatusToAuthorized()
    {
        // Arrange
        var paymentReceivedEvent = new PaymentReceivedEvent
        {
            Id = Guid.NewGuid(),
        };
        var context = new Mock<ConsumeContext<PaymentReceivedEvent>>();
        context
            .SetupGet(x => x.Message)
            .Returns(paymentReceivedEvent);
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Processing, null, default))
            .Returns(Task.CompletedTask);
        _paymentServiceMock
            .Setup(x => x.ExecuteAsync(paymentReceivedEvent, default))
            .Returns(Task.CompletedTask);
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Authorized, null, default))
            .Returns(Task.CompletedTask);
        var sut = new PaymentReceivedEventConsumer(
            _transactionStatusRepositoryMock.Object,
            _paymentServiceMock.Object,
            _loggerMock);

        // Act
        await sut.Consume(context.Object);

        // Assert
        _paymentServiceMock.Verify();
        _transactionStatusRepositoryMock.Verify();
    }

    [Fact]
    public async Task Consume_WithRejectedPayment_ShouldSetStatusToRejected()
    {
        // Arrange
        var paymentReceivedEvent = new PaymentReceivedEvent
        {
            Id = Guid.NewGuid(),
        };
        var context = new Mock<ConsumeContext<PaymentReceivedEvent>>();
        context
            .SetupGet(x => x.Message)
            .Returns(paymentReceivedEvent);
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Processing, null, default))
            .Returns(Task.CompletedTask);
        _paymentServiceMock
            .Setup(x => x.ExecuteAsync(paymentReceivedEvent, default))
            .ThrowsAsync(new PaymentRejectedException("Invalid payment"));
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Rejected, "Invalid payment", default))
            .Returns(Task.CompletedTask);
        var sut = new PaymentReceivedEventConsumer(
            _transactionStatusRepositoryMock.Object,
            _paymentServiceMock.Object,
            _loggerMock);

        // Act
        await sut.Consume(context.Object);

        // Assert
        _paymentServiceMock.Verify();
        _transactionStatusRepositoryMock.Verify();
    }

    [Fact]
    public async Task Consume_WithFailedPayment_ShouldSetStatusToError()
    {
        // Arrange
        var paymentReceivedEvent = new PaymentReceivedEvent
        {
            Id = Guid.NewGuid(),
        };
        var context = new Mock<ConsumeContext<PaymentReceivedEvent>>();
        context
            .SetupGet(x => x.Message)
            .Returns(paymentReceivedEvent);
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Processing, null, default))
            .Returns(Task.CompletedTask);
        _paymentServiceMock
            .Setup(x => x.ExecuteAsync(paymentReceivedEvent, default))
            .ThrowsAsync(new Exception("Internal server error"));
        _transactionStatusRepositoryMock
            .Setup(x => x.SetStatusAsync(paymentReceivedEvent.Id, PaymentStatus.Error, "Internal server error", default))
            .Returns(Task.CompletedTask);
        var sut = new PaymentReceivedEventConsumer(
            _transactionStatusRepositoryMock.Object,
            _paymentServiceMock.Object,
            _loggerMock);

        // Act
        await sut.Consume(context.Object);

        // Assert
        _paymentServiceMock.Verify();
        _transactionStatusRepositoryMock.Verify();
    }
}
