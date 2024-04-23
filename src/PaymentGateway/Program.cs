using PaymentGateway.Features.Balances.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseConsoleLifetime();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDefaultServices(builder.Configuration);
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddMassTransitServices(builder.Configuration);
builder.Services.AddFeaturesServices();

var app = builder.Build();

app.UseWhen(
    predicate: _ => app.Environment.IsDevelopment(),
    configuration: ApplicationBuilderExtensions.UseDevelopmentMiddlewares);

app.UseExceptionHandler();
app.UseHealthChecksProbes();
app.UseRateLimiter();
app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

var v1 = app.MapGroup("/api/v1");
v1.MapPaymentsEndpoints();
v1.MapBalancesEndpoints();

await app.RunAsync();

public partial class Program { }