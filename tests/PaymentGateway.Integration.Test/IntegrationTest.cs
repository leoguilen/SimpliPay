namespace PaymentGateway.Integration.Test;

[Collection(name: nameof(CustomWebApplicationFactoryCollection))]
public abstract class IntegrationTest
{
    protected HttpClient Client { get; }

    protected ITestOutputHelper Logger { get; }

    public IntegrationTest(
        CustomWebApplicationFactory factory,
        ITestOutputHelper outputHelper)
    {
        Client = factory.Server.CreateClient();
        Logger = outputHelper;

        Client.DefaultRequestHeaders.Add("X-Api-Key", "<!eC8>8nYmKfJnT;B%EJH:J[f{WR5P]o");
    }
}
