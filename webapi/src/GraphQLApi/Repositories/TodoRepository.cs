using System.Linq;
using GraphQLApi.Schema.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GraphQLApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private List<Todo> _todos = new List<Todo>() 
        {
            new Todo { Completed = false, Text = "remote debugging aspnetcore in docker with vscode", Id = Guid.NewGuid() },
            new Todo { Completed = true, Text = "hot reloading for aspnetcore", Id = Guid.NewGuid() },
            new Todo { Completed = true, Text = "connect react with graphql", Id = Guid.NewGuid() }
        };

        public IQueryable<Todo> GetTodos()
        {
            return _todos.AsQueryable(); 
        }

        public async Task AddTodoAsync(Todo todo)
        {
            if (TodoExists(todo.Id)) {
                throw new ArgumentException("Todo with given id already exists.", nameof(todo));
            }

            _todos.Add(todo);
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            if (!TodoExists(todo.Id)) {
                throw new ArgumentException("Can't find todo with given id.", nameof(todo));
            }

            _todos[_todos.IndexOf(_todos.Single(x => x.Id == todo.Id))] = todo;
        }

        public Todo GetTodo(Guid id)
        {
            Todo todo = _todos.SingleOrDefault(todo => todo.Id == id);
            if (todo == null) 
            {
                throw new ArgumentException("Can't find todo with given id.", nameof(id));
            }
            return todo;
        }

        private bool TodoExists(Guid id)
        {
            return _todos.Any(x => x.Id == id);
        }
    }
}