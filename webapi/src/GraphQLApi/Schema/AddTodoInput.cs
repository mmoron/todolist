namespace GraphQLApi.Schema
{
    public class AddTodoInput
    {
        public string Text { get; }

        public AddTodoInput(string text)
        {
            Text = text;
        }
    }
}