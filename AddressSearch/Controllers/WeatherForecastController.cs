using IBM.WMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IbmMqListener
{
    public class MqListenerService : BackgroundService
    {
        private readonly ILogger<MqListenerService> _logger;

        public MqListenerService(ILogger<MqListenerService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MQConnectionFactory()
            {
                HostName = "localhost",
                Port = 1414,
                Channel = "DEV.APP.SVRCONN",
                QueueManager = "QM1",
                TransportType = MQC.TRANSPORT_MQSERIES_MANAGED,
                UserName = "app",
                Password = "passw0rd"
            };

            using var connection = factory.CreateConnection();
            using var session = connection.CreateSession();
            var queue = session.GetQueue("queue:///DEV.QUEUE.1");

            using var consumer = queue.CreateConsumer();
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = consumer.Receive();
                if (message != null)
                {
                    _logger.LogInformation($"Received message: {message.ReadString(message.DataLength)}");
                    message.Acknowledge();
                }
            }
        }
    }
}
