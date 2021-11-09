using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace UserService.Subscriptions {
    public abstract class ConsumerBase : BackgroundService {

        private readonly string _exchange;
        private readonly string _queue;
        private readonly string _routingKey;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger _logger;
        private readonly string _rabbitConnString;

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                //_logger.LogInformation($" [x] consumer received: {content}");
                //var accessToken = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["access_token"]);
                //var refreshToken = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["refresh_token"]);
                var success = await HandleMessage(content, ea.RoutingKey);
                if (success) {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queue, false, consumer);

            return Task.CompletedTask;
        }

        protected void InitRabbitMQ() {
            var factory = new ConnectionFactory { HostName = _rabbitConnString };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchange, ExchangeType.Topic);
            _channel.QueueDeclare(_queue, false, false, false, null);
            _channel.QueueBind(_queue, _exchange, _routingKey, null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected abstract Task<bool> HandleMessage(string message, string routingKey);

        protected void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        protected void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public ConsumerBase(string exchange, string queue, string routingKey, ILoggerFactory loggerFactory, IConfiguration config) {
            _rabbitConnString = config.GetConnectionString("RabbitMQConnection");
            _exchange = exchange;
            _queue = queue;
            _routingKey = routingKey;
            _logger = loggerFactory.CreateLogger<ConsumerBase>();
            InitRabbitMQ();
        }

    }
}
