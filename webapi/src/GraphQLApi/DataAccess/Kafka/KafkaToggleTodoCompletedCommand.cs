using GraphQLApi.Schema.Models;
using System;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess.Kafka
{
    public class KafkaToggleTodoCompletedCommand : IToggleTodoCompletedCommand
    {
        private readonly Guid _id;
        private readonly KafkaTodoProducer _producer;
        private readonly ITodoByIdQuery _todoByIdQuery;

        public KafkaToggleTodoCompletedCommand(KafkaTodoProducer producer, ITodoByIdQuery todoByIdQuery, Guid id)
        {
            _id = id;
            _producer = producer;
            _todoByIdQuery = todoByIdQuery;
        }

        public async Task<Todo> Execute()
        {
            Todo todo = _todoByIdQuery.GetTodo(_id);
            todo.Completed = !todo.Completed;
            await _producer.ProduceAsync(todo);
            return todo;
        }
    }
}
