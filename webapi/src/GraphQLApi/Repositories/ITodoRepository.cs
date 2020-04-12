using System.Linq;
using GraphQLApi.Schema.Models;
using System.Threading.Tasks;
using System;

namespace GraphQLApi.Repositories
{
    public interface ITodoRepository 
    {
        IQueryable<Todo> GetTodos();

        Task<Todo> GetTodoAsync(Guid id);

        Task AddTodoAsync(Todo todo); 

        Task UpdateTodoAsync(Todo todo);
    }
}