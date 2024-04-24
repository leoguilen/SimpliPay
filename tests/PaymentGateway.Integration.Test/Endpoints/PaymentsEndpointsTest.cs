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
    public async Task PostPayments_WithValidRequest_ReturnsAccepted()
    {
        // Arrange
        var request = JsonContent.Create(_fixture.CreateValidPaymentRequest());

        // Act
        var response = await Client.PostAsync("/api/v1/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task GetPaymentDetails_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        Client.DefaultRequestHeaders.Remove("X-Api-Key");

        // Act
        var response = await Client.GetAsync($"/api/v1/payments/{paymentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPaymentDetails_WithInvalidPaymentId_ReturnsNotFound()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/v1/payments/{paymentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPaymentDetails_WithValidPaymentId_ReturnsOk()
    {
        // Arrange
        var paymentRequest = _fixture.CreateValidPaymentRequest();
        var postResponse = await Client.PostAsync("/api/v1/payments", JsonContent.Create(paymentRequest));
        var paymentId = postResponse.Headers.Location!.OriginalString.Split('/').Last();
        var expectedResult = new PaymentResponse(
            Guid.Parse(paymentId),
            new(Guid.Empty, string.Empty),
            new(paymentRequest.Amount.Value, paymentRequest.Amount.Currency),
            new(paymentRequest.PaymentMethod.ToString(), paymentRequest.Card.Number),
            paymentRequest.Description,
            paymentRequest.Date.Date,
            nameof(PaymentStatus.Created)
        );

        // Act
        var response = await Client.GetAsync($"/api/v1/payments/{paymentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<PaymentResponse>()).Should()
            .BeEquivalentTo(expectedResult, options => options
                .Excluding(x => x.Client)
                .Excluding(x => x.PaymentMethod.CardNumber));
    }

    [Fact]
    public async Task GetPayments_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        Client.DefaultRequestHeaders.Remove("X-Api-Key");

        // Act
        var response = await Client.GetAsync($"/api/v1/payments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPayments_WithValidFilter_ReturnsOk()
    {
        // Arrange
        var paymentRequest = _fixture.CreateValidPaymentRequest();
        var postResponse = await Client.PostAsync("/api/v1/payments", JsonContent.Create(paymentRequest));
        var paymentId = postResponse.Headers.Location!.OriginalString.Split('/').Last();
        var expectedResult = new PaymentResponse(
            Guid.Parse(paymentId),
            new(Guid.Empty, string.Empty),
            new(paymentRequest.Amount.Value, paymentRequest.Amount.Currency),
            new(paymentRequest.PaymentMethod.ToString(), paymentRequest.Card.Number),
            paymentRequest.Description,
            paymentRequest.Date.Date,
            nameof(PaymentStatus.Created)
        );

        // Act
        var response = await Client.GetAsync($"/api/v1/payments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<IEnumerable<PaymentResponse>>()).Should()
            .ContainEquivalentOf(expectedResult, options => options
                .Excluding(x => x.Client)
                .Excluding(x => x.PaymentMethod.CardNumber));
    }
}
