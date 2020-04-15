using System.Linq;
using GraphQLApi.Schema.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Confluent.Kafka;
using kafka_stream_core;
using kafka_stream_core.SerDes;
using kafka_stream_core.Stream;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace GraphQLApi.Repositories
{
    // TODO: proper implementation of cennection with Kafka
    // We should probably use Kafka state store and find a way to automatically get data from KTable into it
    // Also, as far as I understand, it should be possible in Kafka streams to write directly to stream to update data
    // find a way to do it and get rid of kafka producer
    // ufortunately there is no library .net supporting kafka streams so we have to use kafka-stream-net from source
    // https://github.com/LGouellec/kafka-stream-net/
    // when using this repo todo list in frontend is not refreshed after adding new todo. probably because when query is rerun 
    // newly added todo was not yet picked up by a stream
    public class KafkaTodoRepository : ITodoRepository
    {
        private readonly string topic;
        private readonly string saslUsername;
        private readonly string saslPassword;
        private readonly string bootstrapServers;

        private IProducer<string, string> _producer;
        readonly Dictionary<Guid, Todo> _todosDictionary = new Dictionary<Guid, Todo>();

        public KafkaTodoRepository(IConfiguration configuration)
        {
            topic = configuration["KafkaSecrets:TodosTopicName"];
            saslUsername = configuration["KafkaSecrets:Username"];
            saslPassword = configuration["KafkaSecrets:Password"];
            bootstrapServers = configuration["KafkaSecrets:BootstrapServers"];
            StartConsumer();
            _producer = CreateProducer();
        }

        public IQueryable<Todo> GetTodos()
        {
            return _todosDictionary.Values.AsQueryable();
        }

        public async Task AddTodoAsync(Todo todo)
        {
            if (TodoExists(todo.Id))
            {
                throw new ArgumentException("Todo with given id already exists.", nameof(todo));
            }

            // TODO: error handling
            var result = await _producer.ProduceAsync(topic, new Message<string, string> { Key = todo.Id.ToString(), Value = JsonSerializer.Serialize(todo) });
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            if (!TodoExists(todo.Id))
            {
                throw new ArgumentException("Can't find todo with given id.", nameof(todo));
            }

            // TODO: error handling
            var result = await _producer.ProduceAsync(topic, new Message<string, string> { Key = todo.Id.ToString(), Value = JsonSerializer.Serialize(todo) });
        }

        public Todo GetTodo(Guid id)
        {
            Todo todo = GetTodos().SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
            {
                throw new ArgumentException("Can't find todo with given id.", nameof(id));
            }
            return todo;
        }

        private bool TodoExists(Guid id)
        {
            return GetTodos().Any(x => x.Id == id);
        }

        private IProducer<string, string> CreateProducer()
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
            var builder = new ProducerBuilder<string, string>(producerConfig);

            // TODO: producer is IDisposable. Dispose of it appropriately
            return builder.Build();
        }

        private void StartConsumer() 
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
                _todosDictionary[Guid.Parse(key)] = todo;
            });

            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);

            stream.Start(source.Token);
        }
    }
}