namespace PaymentProcessor.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDbConnection>(new NpgsqlConnection(configuration.GetConnectionString("Database")));
        services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();
        services.AddScoped<IPayablesRepository, PayablesRepository>();
        services.AddScoped<IClientPaymentMethodsRepository, ClientPaymentMethodsRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IIssuingBankService, MockIssuingBankService>();
        services.AddScoped<IPaymentProcessor, CreditCardPaymentProcessor>();
        services.AddScoped<IPaymentProcessor, DebitCardPaymentProcessor>();
        services.AddScoped<IPaymentService, PaymentService>();
    }
}
