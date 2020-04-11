using GraphQLApi.Schema.Models;

namespace GraphQLApi.Schema
{
    public class AddTodoPayload
    {
        public Todo Todo { get; }

        public AddTodoPayload(Todo todo)
        {
            Todo = todo;
        }
    }
}