using System.Linq;
using GraphQLApi.Schema.Models;
using System.Threading.Tasks;

namespace GraphQLApi.Repositories
{
    public interface ITodoRepository 
    {
        IQueryable<Todo> GetTodos();

        Task AddTodoAsync(Todo todo); 

        Task UpdateTodoAsync(Todo todo);
    }
}