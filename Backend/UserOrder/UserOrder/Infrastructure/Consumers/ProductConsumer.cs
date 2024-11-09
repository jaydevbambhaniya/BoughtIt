using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Model;
using UserOrder.Domain.Repository;

namespace UserOrder.Infrastructure.Consumers
{
    public class ProductConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IConnection _connecton;
        private readonly IModel _channel;
        public ProductConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
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
            Console.WriteLine("Product consumer started");

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
                        if (eventType.Equals("ProductAdded",StringComparison.OrdinalIgnoreCase))
                        {
                            var response = await productRepository.AddProductAsync(((JObject)message.Message).ToObject<Product>());
                            Console.WriteLine($"Product Added - {response}");
                        }
                        else if (eventType.Equals("ProductUpdated", StringComparison.OrdinalIgnoreCase))
                        {
                            var response = await productRepository.UpdateProductAsync(((JObject)message.Message).ToObject<Product>());
                            Console.WriteLine($"Product Updated - {response}");
                        }
                        else if (eventType.Equals("ProductDeleted",StringComparison.OrdinalIgnoreCase))
                        {
                            var response = await productRepository.DeleteProductAsync((string)message.Message);
                            Console.WriteLine($"Product Deleted - {response}");
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

            _channel.BasicConsume("Products", false, consumer);

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            Console.WriteLine("Product consumer stopping...");
        }

    }
}