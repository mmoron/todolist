using Confluent.Kafka;
using GraphQLApi.Schema.Models;
using Microsoft.Extensions.Configuration;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using System;
using System.Text.Json;
using System.Threading;

namespace GraphQLApi.DataAccess.Kafka
{
    public class TodosStream
    {
        private readonly string topic;
        private readonly string saslUsername;
        private readonly string saslPassword;
        private readonly string bootstrapServers;
        private readonly TodosStateStoreAccessor _todosStateStoreAccessor;

        public TodosStream(IConfiguration configuration, TodosStateStoreAccessor todosStateStoreAccessor)
        {
            topic = configuration["KafkaTopics:Todos"];
            saslUsername = configuration["KafkaSecrets:Username"];
            saslPassword = configuration["KafkaSecrets:Password"];
            bootstrapServers = configuration["KafkaSecrets:BootstrapServers"];
            this._todosStateStoreAccessor = todosStateStoreAccessor;
        }

        public void Start()
        {
            // TODO: I guess we should dispose of this stream at some point 

            CancellationTokenSource source = new CancellationTokenSource();

            var config = new StreamConfig<StringSerDes, StringSerDes>();
            config.ApplicationId = "test-app";
            config.BootstrapServers = bootstrapServers;
            config.SaslMechanism = SaslMechanism.ScramSha256;
            config.SaslUsername = saslUsername;
            config.SaslPassword = saslPassword;
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
            config.NumStreamThreads = 1;

            StreamBuilder builder = new StreamBuilder();

            builder.Stream<string, string>(topic).Foreach((key, value) =>
            {
                Todo todo = JsonSerializer.Deserialize<Todo>(value);
                _todosStateStoreAccessor.TodosDictionary[Guid.Parse(key)] = todo;
            });

            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);

            stream.Start(source.Token);
        }
    }
}
