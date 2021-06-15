using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OutingsProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "outingsprocessor",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("outings");
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumedResult = consumer.Consume(); // blocking call. Weird, huh?
                //Process the thing.  Add it to the database, apply your rules, whatever
                // you might also just publish it as another topic - for example,
                // you turn an outing -> outingapproved (stream processing again!)
                _logger.LogInformation("Got an outing: " + consumedResult.Message.Value + " at " + consumedResult.Message.Timestamp.ToString());
            }
        }
    }
}
