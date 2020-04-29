using System.Linq;
using GraphQLApi.Schema.Models;
using System;
using System.Text.Json;

namespace GraphQLApi.DataAccess.Kafka
{
    // when using this repo todo list in frontend is not refreshed after adding new todo. probably because when query is rerun 
    // newly added todo was not yet picked up by a stream
    public class KafkaTodoRepository : ITodoByIdQuery, IAllTodosQuery
    {
        private readonly TodosStream _todosStream;

        public KafkaTodoRepository(TodosStream todosStream)
        {
            this._todosStream = todosStream;
        }

        public IQueryable<Todo> AllTodos()
        {
            return this._todosStream
                    .Store
                    .All()
                    .Select(x => JsonSerializer.Deserialize<Todo>(x.Value))
                    .AsQueryable() 
                ?? Enumerable.Empty<Todo>().AsQueryable();
        }

        public Todo GetTodo(Guid id)
        {
            return JsonSerializer
                .Deserialize<Todo>(this._todosStream.Store.Get(id.ToString()));
        }
    }
}