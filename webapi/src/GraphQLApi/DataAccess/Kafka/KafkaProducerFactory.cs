using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess.Kafka
{
    public class KafkaProducerFactory
    {
        private readonly string saslUsername;
        private readonly string saslPassword;
        private readonly string bootstrapServers;

        public KafkaProducerFactory(IConfiguration configuration)
        {
            saslUsername = configuration["KafkaSecrets:Username"];
            saslPassword = configuration["KafkaSecrets:Password"];
            bootstrapServers = configuration["KafkaSecrets:BootstrapServers"];
        }

        public IProducer<TKey, TValue> CreateProducer<TKey, TValue>()
        {
            var producerConfig = new ProducerConfig
            {
                Acks = Acks.All,
                BootstrapServers = bootstrapServers,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslPassword = saslPassword,
                SaslUsername = saslUsername,
                SecurityProtocol = SecurityProtocol.SaslSsl
            };
            var builder = new ProducerBuilder<TKey, TValue>(producerConfig);

            return builder.Build();
        }
    }
}
