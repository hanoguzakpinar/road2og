using System;
using System.Drawing;
using System.Text;
using System.Text.Json;
using RabbitMq.Watermark.Web.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Watermark.Web.BackgroundServices;

public class ImageWatermarkProcessBackgroundService : BackgroundService
{
    private readonly RabbitMQClientService _rabbit;
    private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
    private IChannel _channel;

    public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbit, ILogger<ImageWatermarkProcessBackgroundService> logger)
    {
        _rabbit = rabbit;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = await _rabbit.ConnectAsync();
        await _channel.BasicQosAsync(0, 1, false);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        await _channel.BasicConsumeAsync(RabbitMQClientService.QueueName, false, consumer);

        consumer.ReceivedAsync += Consumer_Received;
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var createdEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroots/images", createdEvent.ImageName);

            var siteName = "www.hanoguzakpinar.com";

            using var img = Image.FromFile(path);
            using var graphic = Graphics.FromImage(img);

            var font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);
            var textSize = graphic.MeasureString(siteName, font);
            var color = Color.Red;
            var brush = new SolidBrush(color);
            var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));

            graphic.DrawString(siteName, font, brush, position);

            img.Save("wwwroot/images/watermarks/" + createdEvent.ImageName);

            img.Dispose();
            graphic.Dispose();

            await _channel.BasicAckAsync(@event.DeliveryTag, false);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
