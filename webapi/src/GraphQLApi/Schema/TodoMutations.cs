using System;
using System.Threading.Tasks;
using GraphQLApi.DataAccess;
using GraphQLApi.Schema.Models;

namespace GraphQLApi.Schema
{
    public class TodoMutations
    {
        private readonly Func<string, IAddTodoCommand> _addTodoCommandFactory;
        Func<Guid, IToggleTodoCompletedCommand> _toggleTodoCompletedCommandFactory;

        public TodoMutations(Func<string, IAddTodoCommand> addTodoCommandFactory, Func<Guid, IToggleTodoCompletedCommand> toggleTodoCompletedCommandFactory)
        {
            _addTodoCommandFactory = addTodoCommandFactory;
            _toggleTodoCompletedCommandFactory = toggleTodoCompletedCommandFactory;
        }

        public async Task<TodoPayload> AddTodoAsync(AddTodoInput input)
        {
            Todo todo = await _addTodoCommandFactory(input.Text).Execute();
            return new TodoPayload(todo);
        }

        public async Task<TodoPayload> ToggleTodoCompleted(ToggleTodoCompletedInput input)
        {
            Todo todo = await _toggleTodoCompletedCommandFactory(input.Id).Execute();
            return new TodoPayload(todo);
        }
    }
}