namespace PaymentGateway.Integration.Test.Endpoints;

[Trait("Category", "IntegrationTest")]
public class PaymentsEndpointsTest : IntegrationTest
{
    private static readonly PaymentRequestFixture _fixture = new();

    public PaymentsEndpointsTest(
        CustomWebApplicationFactory factory,
        ITestOutputHelper outputHelper)
        : base(factory, outputHelper)
    {
        _fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
    }

    [Fact]
    public async Task PostPayments_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var request = JsonContent.Create(_fixture.Create<PaymentRequest>());
        Client.DefaultRequestHeaders.Remove("X-Api-Key");

        // Act
        var response = await Client.PostAsync("/api/v1/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostPayments_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = JsonContent.Create(_fixture.Create<PaymentRequest>());

        // Act
        var response = await Client.PostAsync("/api/v1/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostPayments_WithServiceFailure_ReturnsUnprocessableEntity()
    {
        // Arrange
        var request = JsonContent.Create(_fixture.CreateSuspiciousPaymentRequest());

        // Act
        var response = await Client.PostAsync("/api/v1/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PostPayments_WithValidRequest_ReturnsAccepted()
    {
        // Arrange
        var request = JsonContent.Create(_fixture.CreateValidPaymentRequest());

        // Act
        var response = await Client.PostAsync("/api/v1/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
}
