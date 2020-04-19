using GraphQLApi.Schema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.DataAccess
{
    public interface ITodoByIdQuery
    {
        Todo GetTodo(Guid id);
    }
}
