var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddMassTransit(config =>
{
    config.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    config.AddRider(rider =>
    {
        rider.AddConsumer<PaymentReceivedEventConsumer>();
        rider.UsingKafka((context, k) =>
        {
            var kafkaConfiguration = builder.Configuration.GetRequiredSection("Kafka");

            k.Host(kafkaConfiguration["Server"]);
            k.TopicEndpoint<string, PaymentReceivedEvent>(kafkaConfiguration["TopicName"], kafkaConfiguration["GroupId"], e =>
            {
                e.AutoOffsetReset = AutoOffsetReset.Earliest;
                e.ConcurrentConsumerLimit = 10;

                e.CreateIfMissing(c =>
                {
                    c.NumPartitions = 1;
                    c.ReplicationFactor = 1;
                });
                e.ConfigureConsumer<PaymentReceivedEventConsumer>(context);
            });
        });
    });
});

var host = builder.Build();

await host.RunAsync();
