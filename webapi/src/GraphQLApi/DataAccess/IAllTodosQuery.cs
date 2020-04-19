using GraphQLApi.Schema.Models;
using System.Linq;

namespace GraphQLApi.DataAccess
{
    public interface IAllTodosQuery
    {
        IQueryable<Todo> AllTodos();
    }
}
