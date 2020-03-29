using System.Linq;
using GraphQLApi.Schema.Models;

namespace GraphQLApi.Schema
{
    public class GetTodosQuery
    {
        public IQueryable<Todo> GetTodos(bool? completed) {
            var data = new[] {
                new Todo {Completed = false, Text = "hot reloading for aspnetcore"},
                new Todo {Completed = true, Text = "connect react with graphql"}
            };
            return data.Where(x => completed == null || x.Completed == completed).AsQueryable();
        }
    }
}