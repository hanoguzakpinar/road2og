using FileCreateWorker;
using FileCreateWorker.Models;
using FileCreateWorker.Services;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDbContext<AdventureWorksLt2019Context>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var host = builder.Build();
host.Run();
