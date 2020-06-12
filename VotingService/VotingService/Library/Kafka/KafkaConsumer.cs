using Confluent.Kafka;
using IdentityService.Library.Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Library.Kafka
{
    public class KafkaConsumer
    {
        private String topic;

        private ConsumerConfig consumerConfig;

        private IConsumer<Null, String> consumer;

        public KafkaConsumer(String topic)
        {
            this.topic = topic;
            consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "voting.service.consumer",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoOffsetStore = false
            };
            consumer = new ConsumerBuilder<Null, String>(consumerConfig).Build();
            consumer.Subscribe(this.topic);
        }
       
        public IConsumer<Null, String> GetConsumer()
        {
            return consumer;
        }
    }
}
