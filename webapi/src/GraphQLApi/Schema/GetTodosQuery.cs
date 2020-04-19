using System.Linq;
using GraphQLApi.DataAccess;
using GraphQLApi.Schema.Models;
using HotChocolate;

namespace GraphQLApi.Schema
{
    public class GetTodosQuery
    {
        public IQueryable<Todo> GetTodos(bool? completed, [Service]IAllTodosQuery allTodosQuery) {
            return allTodosQuery.AllTodos()
                .Where(x => completed == null || x.Completed == completed)
                .AsQueryable()
                .OrderBy(x => x.Completed);
        }

        public IQueryable<Todo> GetCompletedTodos([Service]IAllTodosQuery allTodosQuery) {
            return GetTodos(true, allTodosQuery);
        }
    }
}