var builder = WebApplication.CreateBuilder(args);

builder.Host.UseConsoleLifetime();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDefaultServices();
builder.Services.AddValidators();

var app = builder.Build();

app.UseWhen(
    predicate: _ => app.Environment.IsDevelopment(),
    configuration: ApplicationBuilderExtensions.UseDevelopmentMiddlewares);

app.UseExceptionHandler();

app.MapPaymentsEndpoints();

app.UseHealthChecks("/live");
app.UseHealthChecks("/ready");

await app.RunAsync();