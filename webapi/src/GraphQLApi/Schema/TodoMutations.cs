using System;
using System.Threading.Tasks;
using GraphQLApi.Repositories;
using GraphQLApi.Schema.Models;
using HotChocolate;

namespace GraphQLApi.Schema
{
    public class TodoMutations
    {
        public async Task<AddTodoPayload> AddTodoAsync(AddTodoInput input, [Service]ITodoRepository todoRepository)
        {
            Todo newTodo = new Todo() { Text = input.Text, Completed = false, Id = Guid.NewGuid() };
            await todoRepository.AddTodoAsync(newTodo);
            return new AddTodoPayload(newTodo);
        }
    }
}