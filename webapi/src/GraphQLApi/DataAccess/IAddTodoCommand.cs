using GraphQLApi.Schema.Models;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess
{
    public interface IAddTodoCommand
    {
        Task<Todo> Execute();
    }
}
