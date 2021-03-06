﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.State;
using Streamiz.Kafka.Net.State.Internal;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;
using System;
using System.Threading;

namespace GraphQLApi.DataAccess.Kafka
{
    public class TodosStream
    {
        private readonly string topic;
        private readonly string saslUsername;
        private readonly string saslPassword;
        private readonly string bootstrapServers;
        private KafkaStream? _stream;
        private ReadOnlyKeyValueStore<string, string>? _store;

        public TodosStream(IConfiguration configuration)
        {
            topic = configuration["KafkaTopics:Todos"];
            saslUsername = configuration["KafkaSecrets:Username"];
            saslPassword = configuration["KafkaSecrets:Password"];
            bootstrapServers = configuration["KafkaSecrets:BootstrapServers"];
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

            // TODO: this should be a global store I guess, but it is not supported in Streamiz.Kafka.Net yet
            // for now we use topic with one partition to get all the data in one StreamTask
            builder
                // TODO: we need to provide serdes for key anc value as Streamiz.Kafka.Net tries to deserialize
                // value with deserializer for ValueAndTimestamp<string> which is incorrect
                .Table(topic, new StringSerDes(), new StringSerDes(), InMemory<string, string>.As("topics-store")
                .WithCachingDisabled());

            Topology t = builder.Build();
            _stream = new KafkaStream(t, config);

            _stream.Start(source.Token);
        }

        public ReadOnlyKeyValueStore<string, string> Store
        {
            get
            {
                if (_stream == null)
                {
                    throw new InvalidOperationException("Stream doesn't exist yet. Call Start() before getting Store.");
                }
                if (_store == null)
                {
                    var queryableStoreType = QueryableStoreTypes.KeyValueStore<string, string>();
                    _store = _stream.Store(StoreQueryParameters.FromNameAndType("topics-store", queryableStoreType));
                }
                return _store;
            }
        }
    }
}
