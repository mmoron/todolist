using System;

namespace GraphQLApi.Schema
{
    public class ToggleTodoCompletedInput
    {
        public Guid Id { get; }

        public ToggleTodoCompletedInput(Guid id)
        {
            Id = id;
        }
    }
}