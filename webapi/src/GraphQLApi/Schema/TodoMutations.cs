using System;
using System.Threading.Tasks;
using GraphQLApi.Repositories;
using GraphQLApi.Schema.Models;
using HotChocolate;

namespace GraphQLApi.Schema
{
    public class TodoMutations
    {
        public async Task<TodoPayload> AddTodoAsync(AddTodoInput input, [Service]ITodoRepository todoRepository)
        {
            Todo newTodo = new Todo() { Text = input.Text, Completed = false, Id = Guid.NewGuid() };
            await todoRepository.AddTodoAsync(newTodo);
            return new TodoPayload(newTodo);
        }

        public async Task<TodoPayload> ToggleTodoCompleted(ToggleTodoCompletedInput input, [Service]ITodoRepository todoRepository)
        {
            Todo todo = await todoRepository.GetTodoAsync(input.Id);
            todo.Completed = !todo.Completed;
            await todoRepository.UpdateTodoAsync(todo);
            return new TodoPayload(todo);
        }
    }
}