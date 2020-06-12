using Confluent.Kafka;
using IdentityService.Library.Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Library.Kafka
{
    public class KafkaProducer
    {
        private String topic;

        private IProducer<Null, String> producer;

        private ProducerConfig config;

        private ILoggerManager logger;

        public KafkaProducer(String topic, ILoggerManager logger)
        {
            this.topic = topic;
            this.logger = logger;
            config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };
            producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendToKafka(String message)
        {
            var res = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            producer.Flush(TimeSpan.FromSeconds(10));
            logger.LogInfo($"KAFKA => Delivered '{res.Value}' to '{res.TopicPartitionOffset}'");
        }
    }
}
