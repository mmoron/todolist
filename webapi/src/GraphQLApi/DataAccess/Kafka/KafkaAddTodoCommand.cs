using GraphQLApi.Schema.Models;
using System;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess.Kafka
{
    public class KafkaAddTodoCommand : IAddTodoCommand
    {
        private readonly string _text;
        private readonly KafkaTodoProducer _producer;

        public KafkaAddTodoCommand(KafkaTodoProducer producer, string text)
        {
            _text = text;
            _producer = producer;
        }

        public async Task<Todo> Execute()
        {
            Todo todo = new Todo() { Text = _text, Completed = false, Id = Guid.NewGuid() };
            await _producer.ProduceAsync(todo);
            return todo;
        }
    }
}
