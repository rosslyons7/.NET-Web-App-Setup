using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuthorisationService.Producers {
    public class RabbitProducer : IRabbitProducer {
        private readonly string _connString;

        public void ProduceMessage(object message, string exchange, string routingKey) {
            var body = JsonConvert.SerializeObject(message);
            var factory = new ConnectionFactory() { HostName = _connString };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(body.ToArray()));
        }

        public RabbitProducer(IConfiguration config) {

            _connString = config.GetConnectionString("RabbitMQConnection");
        }
    }
}
