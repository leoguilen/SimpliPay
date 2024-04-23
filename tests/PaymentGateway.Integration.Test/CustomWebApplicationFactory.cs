namespace PaymentGateway.Integration.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly ContainersFixture _containersFixture = new();

    public async Task InitializeAsync()
        => await _containersFixture.InitializeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(new ConfigurationBuilder()
            .AddInMemoryCollection([
                new KeyValuePair<string, string?>("ConnectionStrings:Database", _containersFixture.PostgreSqlContainer.GetConnectionString()),
                new KeyValuePair<string, string?>("Kafka:Server", _containersFixture.KafkaContainer.GetBootstrapAddress()),
            ])
            .Build());
        builder.UseTestServer();
    }

    async Task IAsyncLifetime.DisposeAsync()
        => await _containersFixture.DisposeAsync();
}

[CollectionDefinition(nameof(CustomWebApplicationFactoryCollection))]
public class CustomWebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactory>;
