using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Consumers
{
    public class InventoryConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IConnection _connecton;
        private readonly IModel _channel;

        public InventoryConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };
            _connecton = factory.CreateConnection();
            _channel = _connecton.CreateModel();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Inventory consumer started");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(body));
                Console.WriteLine(message);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                        string eventType = (string)message.EventType;
                        if (eventType.Equals("QuantityUpdate", StringComparison.OrdinalIgnoreCase))
                        {
                            List<(string GlobalProductId, int Quantity)> data = new();
                            foreach (var item in message.Message)
                            {
                                data.Add((item.GlobalProductId,item.Quantity));
                            }
                            bool response = await productRepository.UpdateProductQuantity(data);
                            Console.WriteLine($"Quantity Update - {response}");
                        }
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume("Inventory", false, consumer);

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            Console.WriteLine("Inventory consumer stopping...");
        }
    }
}
