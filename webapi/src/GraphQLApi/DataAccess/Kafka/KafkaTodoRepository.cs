using System.Linq;
using GraphQLApi.Schema.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace GraphQLApi.DataAccess.Kafka
{
    // when using this repo todo list in frontend is not refreshed after adding new todo. probably because when query is rerun 
    // newly added todo was not yet picked up by a stream
    public class KafkaTodoRepository : ITodoByIdQuery, IAllTodosQuery
    {
        private readonly TodosStateStoreAccessor _todosStateStoreAccessor;

        public KafkaTodoRepository(IConfiguration configuration, TodosStateStoreAccessor todosStateStoreAccessor)
        {
            this._todosStateStoreAccessor = todosStateStoreAccessor;
        }

        public IQueryable<Todo> AllTodos()
        {
            return _todosStateStoreAccessor.TodosDictionary.Values.AsQueryable();
        }

        public Todo GetTodo(Guid id)
        {
            Todo todo = AllTodos().SingleOrDefault(todo => todo.Id == id);
            if (todo == null)
            {
                throw new ArgumentException("Can't find todo with given id.", nameof(id));
            }
            return todo;
        }
    }
}