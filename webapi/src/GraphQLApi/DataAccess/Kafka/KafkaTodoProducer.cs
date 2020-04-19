using Confluent.Kafka;
using GraphQLApi.Schema.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess.Kafka
{
    public class KafkaTodoProducer
    {
        private readonly string _topic;
        private readonly IProducer<string, string> _producer;

        public KafkaTodoProducer(IConfiguration configuration, KafkaProducerFactory producerFactory)
        {
            _topic = configuration["KafkaTopics:Todos"];

            //TODO: producer is IDisposable.Dispose of it appropriately
            _producer = producerFactory.CreateProducer<string, string>();
        }

        public async Task<DeliveryResult<string, string>> ProduceAsync(Todo todo)
        {
            return await _producer
                .ProduceAsync(_topic, new Message<string, string> { 
                    Key = todo.Id.ToString(), 
                    Value = JsonSerializer.Serialize(todo) 
                });
        }
    }
}
