using DistributedLibrary.Books.Service.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedLibrary.Books.Service {
    public class Worker:BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private DataService _data;
        public Worker(ILogger<Worker> logger) {
            _logger = logger;
            var factory = new ConnectionFactory {
                HostName = "localhost",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("send-ibsn", false, false, false, null);
            _consumer = new EventingBasicConsumer(_channel);
            _data = new DataService();
        }

        public override Task StartAsync(CancellationToken cancellationToken) {
           
            _consumer.Received += async (model, content) => {
                long value = 0;
                var body = content.Body.ToArray();
                var isbn = Encoding.UTF8.GetString(body);
                value = long.Parse(isbn);
                await SearchIsbn(value);

            };
            _channel.BasicConsume("send-ibsn", true, _consumer);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(60000, stoppingToken);
            }
        }

        private void NotifyPaymentDone() {
            var factory = new ConnectionFactory {
                HostName = "localhost",
                Port = 5672
            };
            using (var connection = factory.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare("receive-payment", false, false, false, null);
                    var message = "Payment Successful!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "receive-payment", null, body);
                }
            }
        }
        private async Task<object> SearchIsbn(long basketId) {
            var data = _data.GetEntityById(basketId);
            if (data == null) {
                return null;
            }
            HttpClient Client = new HttpClient();
            var result = await Client.GetStringAsync($"http://localhost:52611/api/Authors/{data.AuthorId}");
            if (result != "") {
                var result1 = JsonConvert.DeserializeObject<Authors>(result);
                await ReceiveFilename(data, result1);
                return result;
            }
            var result2 = await Client.GetStringAsync($"http://localhost:54376/api/Authors/{data.AuthorId}");
            if (result2 != "") {
                var resultObject = JsonConvert.DeserializeObject<Authors>(result2);
                await ReceiveFilename(data,resultObject);
                return result;
            }
            return null;
        }
       

        public async Task ReceiveFilename(Services.Books libro,Authors autor) {
            string archivo = "";
            var factory = new ConnectionFactory {
                HostName = "localhost",
                Port = 5672,
            };
            IConnection _connection = factory.CreateConnection();
            IModel _channel = _connection.CreateModel();
            _channel.QueueDeclare("send-archivo", false, false, false, null);
            EventingBasicConsumer _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += async (model, Content) => {
                var body = Content.Body.ToArray();
                archivo = Encoding.UTF8.GetString(body);
                string result = JsonConvert.SerializeObject(libro) + "\n" + JsonConvert.SerializeObject(autor);
                await File.WriteAllTextAsync(archivo, result);
            };
            _channel.BasicConsume("send-archivo", true, _consumer);
        }
    } 
}
