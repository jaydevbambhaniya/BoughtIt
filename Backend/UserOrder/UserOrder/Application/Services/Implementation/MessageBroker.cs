using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Services.Implementation
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IConfiguration _configuration;
        public MessageBroker(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> PublishMessageAsync<T>(string topic, T message, string eventType)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };
            var data = new { EventType = eventType, Message = message };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclareNoWait(queue: topic, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var serializedMessage = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(serializedMessage);
                Console.WriteLine(serializedMessage);
                channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, body: body);

                return await Task.FromResult(true);
            }
        }
    }
}
