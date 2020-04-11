using System.Linq;
using GraphQLApi.Repositories;
using GraphQLApi.Schema.Models;
using HotChocolate;

namespace GraphQLApi.Schema
{
    public class GetTodosQuery
    {
        public IQueryable<Todo> GetTodos(bool? completed, [Service]ITodoRepository todoRepository) {
            return todoRepository.GetTodos()
                .Where(x => completed == null || x.Completed == completed)
                .AsQueryable()
                .OrderBy(x => x.Completed);
        }

        public IQueryable<Todo> GetCompletedTodos([Service]ITodoRepository todoRepository) {
            return GetTodos(true, todoRepository);
        }
    }
}