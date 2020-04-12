using GraphQLApi.Schema.Models;

namespace GraphQLApi.Schema
{
    public class TodoPayload
    {
        public Todo Todo { get; }

        public TodoPayload(Todo todo)
        {
            Todo = todo;
        }
    }
}