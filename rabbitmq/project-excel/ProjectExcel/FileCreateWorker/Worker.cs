using Azure.Core;
using ClosedXML.Excel;
using FileCreateWorker.Models;
using FileCreateWorker.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FileCreateWorker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly RabbitMQClientService _rabbit;
		private readonly IServiceProvider _serviceProvider;

		private IChannel _channel;

		//private readonly AdventureWorksLt2019Context _db;
		// context böyle alınamaz DI'dan
		//DbContext varsayılan olarak Scoped yaşam döngüsüyle kayıtlıdır. Ancak BackgroundService bir Singleton'dır. .NET, Singleton içine doğrudan Scoped servis enjekte etmeye izin vermez.

		public Worker(ILogger<Worker> logger, RabbitMQClientService rabbit, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_rabbit = rabbit;
			_serviceProvider = serviceProvider;
		}
		public override async Task StartAsync(CancellationToken cancellationToken)
		{
			_channel = await _rabbit.ConnectAsync();
			await _channel.BasicQosAsync(0, 1, true);

			_logger.LogInformation($"Connection açıldı.");

			await base.StartAsync(cancellationToken);

			while(!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(1000, cancellationToken);
            }
        }
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new AsyncEventingBasicConsumer(_channel);

			consumer.ReceivedAsync += Consumer_ReceivedAsync;

			await _channel.BasicConsumeAsync(queue: RabbitMQClientService.QueueName, autoAck: false, consumer: consumer);
		}

		private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
		{
			_logger.LogInformation($"Dinlemeye başlandı.");

			var request = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            Console.WriteLine(Encoding.UTF8.GetString(@event.Body.ToArray()));

			using var ms = new MemoryStream();

			var wb = new XLWorkbook();
			var ds = new DataSet();

			ds.Tables.Add(GetTable("products"));

			wb.Worksheets.Add(ds);
			wb.SaveAs(ms);

			MultipartFormDataContent multipartFormDataContent = new();
			multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", Guid.NewGuid().ToString() + ".xlsx");

			var baseUrl = "http://localhost:5164/api/files";

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsync($"{baseUrl}?fileId={request.FileId}", multipartFormDataContent);
				if (response.IsSuccessStatusCode)
				{
					await _channel.BasicAckAsync(@event.DeliveryTag, false);
					_logger.LogInformation($"File with id {request.FileId} created successfully.");
				}
			}
		}

		private DataTable GetTable(string tableName)
		{
			List<Product> products;

			using (var scope = _serviceProvider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<AdventureWorksLt2019Context>();
				products = dbContext.Products.ToList();
			}

			var table = new DataTable(tableName);
			table.Columns.Add("ProductId", typeof(int));
			table.Columns.Add("Name", typeof(string));
			table.Columns.Add("ProductNumber", typeof(string));
			table.Columns.Add("Color", typeof(string));

			products.ForEach(p =>
			{
				table.Rows.Add(p.ProductId, p.Name, p.ProductNumber, p.Color);
			});

			return table;
		}
	}
}
