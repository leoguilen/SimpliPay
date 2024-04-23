namespace PaymentGateway.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options
                .SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway API",
                    Description =
                        "A simple payment gateway for SimpliPay.\n" +
                        "This service is used as an easy integration interface for merchants to process payments and view their accounts receivable.",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "SimpliPay",
                        Email = "support@simplipay.com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    },
                });

            options
                .AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key authentication",
                    Name = "X-Api-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme",
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        In = ParameterLocation.Header,
                    },
                    Array.Empty<string>()
                }
            });

            options.SupportNonNullableReferenceTypes();
        });

        return services;
    }

    public static IServiceCollection AddDefaultServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddProblemDetails()
            .AddDataProtectionServices()
            .AddHttpContextAccessor()
            .AddMemoryCache()
            .AddScoped<IClientContext, ClientContext>()
            .AddScoped<IApiKeyValidation, ApiKeyValidation>()
            .AddTransient<ApiKeyAuthenticationMiddleware>()
            .AddExceptionHandler<GlobalExceptionHandlerMiddleware>()
            .AddRateLimiterServices()
            .AddHealthChecks(configuration)
            .AddValidators();
    }

    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddSingleton<IDbConnection>(new NpgsqlConnection(configuration.GetConnectionString("Database")))
            .AddScoped<ITransactionsRepository, TransactionsRepository>();
    }

    public static IServiceCollection AddMassTransitServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddMassTransit(cfg =>
        {
            cfg.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            cfg.AddRider(cfg =>
            {
                cfg.AddProducer<string, PaymentReceived>("events.simplipay.payments");
                cfg.UsingKafka((context, cfg) =>
                {
                    var kafkaConfiguration = configuration.GetRequiredSection("Kafka");

                    cfg.Host(kafkaConfiguration["Server"]);

                    cfg.TopicEndpoint<string, PaymentReceived>(
                        kafkaConfiguration["TopicName"],
                        kafkaConfiguration["GroupId"],
                        c =>
                        {
                            c.CreateIfMissing(c =>
                            {
                                c.NumPartitions = 1;
                                c.ReplicationFactor = 1;
                            });
                            c.UseRawJsonSerializer();
                            c.ConfigureError(x => x.UseRetry((r) => r.Immediate(5)));
                            c.ConfigureDefaultErrorTransport();
                        });
                });
            });
        });
    }

    public static IServiceCollection AddFeaturesServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IPaymentRiskCheckService, PaymentRiskCheckService>()
            .AddScoped<IPaymentService, PaymentService>();
    }

    private static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
    {
        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                context.HttpContext.Response.StatusCode = options.RejectionStatusCode;
                return new ValueTask();
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var contains = httpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey);
                return !contains
                    ? RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: string.Empty,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(5),
                            QueueLimit = 2,
                        })
                    : RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: apiKey.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 50,
                            Window = TimeSpan.FromSeconds(2),
                            QueueLimit = 10,
                        });
            });
        });
    }

    private static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddHealthChecks()
            .AddApplicationStatus(
                name: "Application",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["app", "ready", "live"],
                timeout: TimeSpan.FromSeconds(5)
            )
            .AddNpgSql(
                connectionString: configuration.GetConnectionString("Database") ?? string.Empty,
                healthQuery: "SELECT * FROM pg_stat_database where datname = 'SimpliPay';",
                configure: null,
                name: "Database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "postgresql", "ready"],
                timeout: TimeSpan.FromSeconds(5)
            )
            .AddKafka(
                config: new ProducerConfig()
                {
                    BootstrapServers = configuration["Kafka:Server"],
                },
                name: "Kafka",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["kafka", "ready"],
                timeout: TimeSpan.FromSeconds(5)
            )
            .Services;
    }

    private static IServiceCollection AddDataProtectionServices(this IServiceCollection services)
    {
        return services
            .AddDataProtection()
            .SetApplicationName("PaymentGateway")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
            // .ProtectKeysWithCertificate()
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256,
            })
            .Services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<PaymentRequest>, PaymentRequestValidator>();
    }
}
