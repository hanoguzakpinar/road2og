using FileCreateWorker;
using FileCreateWorker.Services;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(sp =>
{
    return new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMq"))
    };
});
builder.Services.AddSingleton<RabbitMQClientService>();

var host = builder.Build();
host.Run();
