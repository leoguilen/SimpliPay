using ClientResponse = PaymentGateway.Features.Balances.Endpoints.Contracts.Responses.ClientResponse;

namespace PaymentGateway.Integration.Test.Endpoints;

[Trait("Category", "Integration")]
public class BalancesEndpointsTest(
    CustomWebApplicationFactory factory,
    ITestOutputHelper outputHelper)
    : IntegrationTest(factory, outputHelper)
{
    [Fact]
    public async Task GetBalances_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        Client.DefaultRequestHeaders.Remove("X-Api-Key");

        // Act
        var response = await Client.GetAsync("/api/v1/balances");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetBalances_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var expectedResponse = new BalanceSummaryResponse(
            new ClientResponse(Guid.Parse("875cce09-eb96-4ed2-bab2-728b40cc0a98"), ""),
            [
                new BalanceResponse(nameof(PayableStatus.Paid), 0, 0),
                new BalanceResponse(nameof(PayableStatus.WaitingFunds), 0, 0)
            ]);

        // Act
        var response = await Client.GetAsync("/api/v1/balances");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<BalanceSummaryResponse>())
            .Should().BeEquivalentTo(expectedResponse);
    }
}
