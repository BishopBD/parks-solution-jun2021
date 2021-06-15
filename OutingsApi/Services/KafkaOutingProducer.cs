using Confluent.Kafka;
using OutingsApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutingsApi.Services
{
    public class KafkaOutingProducer : IWriteOutingsForProcessing
    {
        IProducer<Null, string> _producer;
        public KafkaOutingProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendOuting(PostOutingCreateRequest request)
        {

            var json = JsonSerializer.Serialize(request);
            await _producer.ProduceAsync("outings", new Message<Null, string> { Value = json });
        }
    }
}
